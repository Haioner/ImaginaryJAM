using UnityEngine.UI;
using static Models;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("CACHE")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform feetTransform;
    [SerializeField] private Camera cam;
    [SerializeField] private Slider staminaSlider;

    private DefaultInput _defaultInput;
    private CharacterController _characterController;

    //Movement
    private Vector3 _newMovementSpeed;
    private Vector3 _newMovementSpeedVelocity;
    public bool IsSprinting { private set; get; }
    private float _currentStamina;
    private bool _chargingStamina;
    public float CurrentSpeed { private set; get; }
    public Vector2 InputMovement { private set; get; }
    public Vector2 currentInputVector { private set; get; }
    private Vector2 smoothInputVelocity;

    //Gravity
    private float _playerGravity;
    private Vector3 _jumpingForce;
    private Vector3 _jumpingForceVelocity;
    private bool _isCrouching;
    public bool IsGrounded { private set; get; }

    //View
    private float _initialCamFov;
    private Vector3 _newCameraRotation;
    private Vector3 _newCharacterRotation;
    public Vector2 InputView { private set; get; }

    [Header("Settings")]
    [SerializeField] private ViewSettings viewSettings; [Space]
    [SerializeField] private MovementSettings movementSettings; [Space]
    [SerializeField] private GravitySettings gravitySettings;

    [Header("Stance")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float playerStanceSmoothing;
    public PlayerStance playerStance { private set; get; }

    [Header("Stance Colliders")]
    [SerializeField] CharacterStance playerStandStance;
    [SerializeField] CharacterStance playerCrouchStance;
    [SerializeField] CharacterStance playerProneStance;

    private float _cameraHeight;
    private float _cameraHeightVelocity;
    private float _stanceCheckErrorMargin = 0.05f;
    private Vector3 _stanceCapsuleCenterVelocity;
    private float _stanceCapsuleHeightVelocity;

    #region Methods

    private void Awake()
    {
        //View
        _newCameraRotation = cameraHolder.localRotation.eulerAngles;
        _newCharacterRotation = transform.localRotation.eulerAngles;

        //Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Inputs
        _defaultInput = new DefaultInput();
        _defaultInput.Character.Movement.performed += e => InputMovement = e.ReadValue<Vector2>();
        _defaultInput.View.CameraView.performed += e => InputView = e.ReadValue<Vector2>();
        _defaultInput.Character.Jump.performed += e => Jump();
        _defaultInput.Character.Crouch.performed += e => Crouch();
        _defaultInput.Character.CrouchRelease.performed += e => CrouchRelease();
        _defaultInput.Character.Prone.performed += e => Prone();
        _defaultInput.Enable();

        _characterController = GetComponent<CharacterController>();

        _cameraHeight = cameraHolder.localPosition.y;
        _initialCamFov = cam.fieldOfView;
        _currentStamina = movementSettings.MaxStamina;
    }

    private void Update()
    {
        UpdateMovementMethods();
        SetIsGrounded();
        CalculateView();
    }

    private void FixedUpdate() => FixedMovementMethods();

    #endregion

    #region ControllersMethods

    public void SetSensitivity(float value)
    {
        viewSettings.ViewXSensitivity = value;
        viewSettings.ViewYSensitivity = value;
    }

    public void SetViewState(bool state) => viewSettings.CanView = state;

    public void SetRotations(Vector3 cameraRot, Vector3 character)
    {
        _newCameraRotation = cameraRot;
        _newCharacterRotation = character;
    }

    public Vector3 GetNewCameraRotation()
    {
        return _newCameraRotation;
    }

    public Vector3 GetNewCharacterRotation()
    {
        return _newCharacterRotation;
    }

    public Vector3 GetCameraRotation()
    {
        return cameraHolder.localEulerAngles;
    }

    public Vector3 GetCharacterRotation()
    {
        return transform.localEulerAngles;
    }

    public bool GetViewState() { return  viewSettings.CanView; }

    public void SetMoveState(bool state)
    {
        movementSettings.CanMove = state;
        CurrentSpeed = 0;
    }

    public void SetHorizontalClamp(bool state) => viewSettings.CanClampHorizontal = state;

    public void SetControllerActive(bool state) => gameObject.SetActive(state);

    private void UpdateMovementMethods()
    {
        CalculateStamina();

        if (!movementSettings.CanMove) return;

        CalculateStance();
        UpdateCrouch();
        Sprint();
        CameraSprint();
        SmoothInput();
    }

    private void FixedMovementMethods()
    {
        if (!movementSettings.CanMove) return;

        CalculateCurrentSpeed();
        CalculateMovement();
        CalculateJump();
    }

    #endregion

    #region View

    private void CalculateView()
    {
        if (!viewSettings.CanView) return;

        _newCharacterRotation.y += viewSettings.ViewXSensitivity * (viewSettings.ViewXInverted ? -InputView.x : InputView.x) * Time.deltaTime;
        if (viewSettings.CanClampHorizontal)
            _newCharacterRotation.y = Mathf.Clamp(_newCharacterRotation.y, viewSettings.viewClampHorizontalMin, viewSettings.viewClampHorizontalMax);
        transform.localRotation = Quaternion.Euler(_newCharacterRotation);

        _newCameraRotation.x += viewSettings.ViewYSensitivity * (viewSettings.ViewYInverted ? InputView.y : -InputView.y) * Time.deltaTime;
        _newCameraRotation.x = Mathf.Clamp(_newCameraRotation.x, viewSettings.viewClampVerticalMin, viewSettings.viewClampVerticalMax);

        cameraHolder.localRotation = Quaternion.Euler(_newCameraRotation);
    }

    #endregion

    #region Movement

    public void SmoothInput()
    {
        currentInputVector = Vector2.SmoothDamp(currentInputVector, InputMovement, ref smoothInputVelocity, movementSettings.SmoothInputSpeed);
    }

    private void CalculateMovement()
    {
        //Get Movement
        _newMovementSpeed = Vector3.SmoothDamp(_newMovementSpeed,
            new Vector3(CalculateMoveSpeed().x * currentInputVector.x * Time.deltaTime, 0, CalculateMoveSpeed().y * currentInputVector.y * Time.deltaTime),
            ref _newMovementSpeedVelocity, IsGrounded ? movementSettings.MovementSmoothing : gravitySettings.FallingSmoothing);

        var movementSpeed = transform.TransformDirection(_newMovementSpeed);

        //Get Gravity
        if (_playerGravity > gravitySettings.GravityMin)
            _playerGravity -= gravitySettings.GravityAmount * Time.deltaTime;

        if (_playerGravity < -0.1f && IsGrounded)
            _playerGravity = -0.1f;

        movementSpeed.y += _playerGravity;
        movementSpeed += _jumpingForce * Time.deltaTime;

        //Apply
        _characterController.Move(movementSpeed);
    }

    private Vector2 CalculateMoveSpeed()
    {
        //Walk Speed
        Vector2 speed = new Vector2(movementSettings.WalkingStrafeSpeed, movementSettings.WalkingForwardSpeed);

        //Sprint Speed
        if (IsSprinting && playerStance == PlayerStance.Stand)
            speed = new Vector2(movementSettings.RunningStrafeSpeed, movementSettings.RunningForwardSpeed);

        //Effector Speed
        speed *= SpeedEffector();

        return speed;
    }

    private float SpeedEffector()
    {
        if (!IsGrounded) //Jump Speed
            return movementSettings.FallingSpeedEffector;

        else if (playerStance == PlayerStance.Crouch) //Crouch Speed
            return movementSettings.CrouchSpeedEffector;

        else if (playerStance == PlayerStance.Prone) //Prone Speed
            return movementSettings.ProneSpeedEffector;

        else //Base Speed
            return 1;
    }

    private void Sprint()
    {
        if (_defaultInput.Character.Sprint.ReadValue<float>() != 0 && playerStance == PlayerStance.Stand && !_chargingStamina)
        {
            if (CurrentSpeed > 0)
                IsSprinting = true;
            else
                IsSprinting = false;
        }
        else
            IsSprinting = false;
    }

    private void CalculateStamina()
    {
        if (!movementSettings.CanStamina)
        {
            if (staminaSlider != null)
                staminaSlider.gameObject.SetActive(false);
            return;
        }
        //Calculate
        if (_currentStamina > 0 && IsSprinting && !_chargingStamina && movementSettings.CanMove)
        {
            _currentStamina -= Time.deltaTime * movementSettings.StaminaWasteVelocity;
            staminaSlider.gameObject.SetActive(true);
        }
        else if (_currentStamina < movementSettings.MaxStamina)
        {
            _currentStamina += Time.deltaTime * movementSettings.StaminaChargeVelocity;
        }

        //Panting
        if (_currentStamina <= 0)
        {
            _chargingStamina = true;

            if (movementSettings.PantingAudioSource != null)
                movementSettings.PantingAudioSource.Play();
            
        }
        else if (_currentStamina >= movementSettings.MinCanRunAfterZero && _chargingStamina)
            _chargingStamina = false;

        //Max Stamina
        if (_currentStamina >= movementSettings.MaxStamina)
        {
            _currentStamina = movementSettings.MaxStamina;
            staminaSlider.gameObject.SetActive(false);
        }

        UpdateStaminaSlider();
    }

    private void UpdateStaminaSlider()
    {
        float speedSliderValue = movementSettings.StaminaWasteVelocity * (movementSettings.MaxStamina / 5) * Time.deltaTime;
        staminaSlider.maxValue = movementSettings.MaxStamina;
        staminaSlider.value = Mathf.MoveTowards(staminaSlider.value, _currentStamina, speedSliderValue);
    }

    private void CameraSprint()
    {
        if (IsSprinting)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, _initialCamFov + movementSettings.FovToAdd, Time.deltaTime * movementSettings.FovVelocity);
    }

    private void CalculateCurrentSpeed()
    {
        CurrentSpeed = _characterController.velocity.magnitude / (movementSettings.WalkingForwardSpeed * SpeedEffector());
        if (CurrentSpeed > 1)
            CurrentSpeed = 1;
    }

    #endregion

    #region Gravity/Jump

    private void SetIsGrounded() => IsGrounded = _characterController.isGrounded;

    private void CalculateJump() => _jumpingForce = Vector3.SmoothDamp(_jumpingForce, Vector3.zero, ref _jumpingForceVelocity, gravitySettings.JumpingFallof);

    public Vector3 GetJumpForce()
    {
        return _jumpingForce;
    }

    private void Jump()
    {
        if (!IsGrounded) return;

        if(playerStance != PlayerStance.Stand)
        {
            if (CheckCollisionAbove(playerStandStance.StanceCollider.height))
                return;

            playerStance = PlayerStance.Stand;
            return;
        }

        _jumpingForce = Vector3.up * gravitySettings.JumpingHeight;
        _playerGravity = 0;
    }

    #endregion

    #region Stance

    public void SetSeated(bool enabled)
    {
        if (enabled)
            playerStance = PlayerStance.Seated;
        else
            playerStance = PlayerStance.Stand;
    }

    private void CalculateStance()
    {
        //Set Camera height
        _cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, GetCurrentStance().CameraHeight, ref _cameraHeightVelocity, playerStanceSmoothing);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, _cameraHeight, cameraHolder.localPosition.z);

        //Set Character height
        _characterController.height =
            Mathf.SmoothDamp(_characterController.height, GetCurrentStance().StanceCollider.height, ref _stanceCapsuleHeightVelocity, playerStanceSmoothing);

        _characterController.center =
            Vector3.SmoothDamp(_characterController.center, GetCurrentStance().StanceCollider.center, ref _stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }

    private CharacterStance GetCurrentStance()
    {
        if (playerStance == PlayerStance.Crouch)
            return playerCrouchStance;
        
        else if (playerStance == PlayerStance.Prone)
            return playerProneStance;
        
        else
            return playerStandStance;
    }

    private void CrouchRelease() => _isCrouching = false;
    private void Crouch()
    {
        if (_defaultInput.Character.Crouch.ReadValue<float>() != 0 && !CheckCollisionAbove(playerCrouchStance.StanceCollider.height))
        {
            playerStance = PlayerStance.Crouch;
            _isCrouching = true;
        }
    }

    private void UpdateCrouch()
    {
        if (playerStance == PlayerStance.Crouch && !CheckCollisionAbove(playerStandStance.StanceCollider.height) && _isCrouching == false)
            playerStance = PlayerStance.Stand;
    }

    private void Prone()
    {
        if (playerStance == PlayerStance.Prone && !CheckCollisionAbove(playerStandStance.StanceCollider.height))
            playerStance = PlayerStance.Stand;
        else
            playerStance = PlayerStance.Prone;
    }

    private bool CheckCollisionAbove(float stanceCheckHeight)
    {
        var start = new Vector3
            (feetTransform.position.x, feetTransform.position.y + _characterController.radius + _stanceCheckErrorMargin, feetTransform.position.z);
        var end = new Vector3
            (feetTransform.position.x, feetTransform.position.y - _characterController.radius - _stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);

        return Physics.CheckCapsule(start, end, _characterController.radius, playerMask);
    }
    #endregion
}
