using System.Collections;
using UnityEngine;

public class ChairController : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactMessage = "Interact";
    public string InteractMessage
    {
        get { return interactMessage; }
        set { interactMessage = value; }
    }

    [Space]
    [SerializeField] private Transform seatPosition;
    [SerializeField] private float lerpSpeed = 3f;
    public bool isActive = false;
    private bool _isSeat = false;

    private PlayerController playerController;
    private PlayerInteract _playerInteract;
    private Vector3 _initialPosition;

    private DefaultInput _defaultInput;
    private void Awake()
    {
        _defaultInput = new DefaultInput();
        _defaultInput.Interactables.InteractButton.performed += e => Exit();
        _defaultInput.Enable();
    }

    public void Interact()
    {
        GetPlayerController();
        isActive = !isActive;
        MovePlayer();
        playerController.SetMoveState(!isActive);
        playerController.SetSeated(isActive);
        Invoke("SetSeat", 0.1f);
    }

    private void Exit()
    {
        if (_isSeat)
            Interact();
    }

    private void SetSeat()
    {
        _isSeat = !_isSeat;
        _playerInteract.enabled = !isActive;
    }

    private void GetPlayerController()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
            _playerInteract = playerController.GetComponent<PlayerInteract>();
        }
    }

    private void MovePlayer()
    {
        if (isActive)
        {
            //Save Player Stand position
            _initialPosition = playerController.transform.localPosition;

            //Move player to chair position
            StartCoroutine(LerpPosTo(playerController.transform.position, seatPosition.transform.position));
            StartCoroutine(LerpRotTo(CalculatedCurrentCamera(), Vector3.zero, CalculatedCurrentCharacter(), seatPosition.eulerAngles));
        }
        else
        {
            //Move player to stand position
            StartCoroutine(LerpPosTo(playerController.transform.position, _initialPosition));
            playerController.transform.SetParent(null);

            StartCoroutine(LerpRotTo(
                CalculatedCurrentCamera(), CalculatedCurrentCamera(),
                playerController.GetCharacterRotation(), CalibratedCharacterRotation()));

            playerController.SetHorizontalClamp(isActive);
        }
    }

    IEnumerator LerpPosTo(Vector3 initialPos, Vector3 targetPos)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            playerController.transform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }
        playerController.transform.position = targetPos;
    }

    private Vector3 CalculatedCurrentCamera()
    {
        Vector3 calculatedInitalCamera = playerController.GetCameraRotation();
        calculatedInitalCamera.x = playerController.GetNewCameraRotation().x;
        return calculatedInitalCamera;
    }

    private Vector3 CalculatedCurrentCharacter()
    {
        Vector3 calculatedInitalCharacter = playerController.GetCharacterRotation();
        calculatedInitalCharacter.y = playerController.GetNewCharacterRotation().y % 360;
        return calculatedInitalCharacter;
    }

    private Vector3 CalibratedCharacterRotation()
    {
        Vector3 CalculatedCharRot = Vector3.zero;
        CalculatedCharRot.y = playerController.GetCharacterRotation().y;
        return CalculatedCharRot;
    }

    IEnumerator LerpRotTo(Vector3 currentCamRot, Vector3 targetCamRot, Vector3 currentCharRot, Vector3 targetCharRot)
    {
        if (targetCamRot != Vector3.zero)
            playerController.SetHorizontalClamp(isActive);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            Vector3 lerpCamRot = LerpAngle(currentCamRot, targetCamRot, elapsedTime);
            Vector3 lerpCharRot = LerpAngle(currentCharRot, targetCharRot, elapsedTime);

            playerController.SetRotations(lerpCamRot, lerpCharRot);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }
        playerController.SetRotations(targetCamRot, targetCharRot);
        playerController.SetHorizontalClamp(isActive);
        Recalculate();
    }

    Vector3 LerpAngle(Vector3 from, Vector3 to, float t)
    {
        return new Vector3(
            Mathf.LerpAngle(from.x, to.x, t),
            Mathf.LerpAngle(from.y, to.y, t),
            Mathf.LerpAngle(from.z, to.z, t)
        );
    }

    private void Recalculate()
    {
        if (isActive)
        {
            playerController.transform.SetParent(transform);
            playerController.SetRotations(CalculatedCurrentCamera(), Vector3.zero);
        }
        else
        {
            playerController.SetRotations(CalculatedCurrentCamera(), CalibratedCharacterRotation());
        }
    }
}