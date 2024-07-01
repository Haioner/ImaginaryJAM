using static Models;
using UnityEngine;

public class CameraHeadbob : MonoBehaviour
{
    [Header("CACHE")]
    [SerializeField] private PlayerController playerController;
    private DefaultInput _defaultInput;

    [Header("HeadBob Settings")]
    [Range(0.001f, 0.05f)] [SerializeField] private float amount = 0.004f;
    [Range(10f, 100f)] [SerializeField] private float smooth = 15f;
    [SerializeField] private float frequency = 15f;

    private float _initialAmount;
    private Vector3 _startPos;

    private void Awake()
    {
        _defaultInput = new DefaultInput();
        _defaultInput.Enable();

        _initialAmount = amount;
        _startPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        UpdateHeadBob();
        StopHeadBob();
    }

    private void SetAmountBySpeed(Vector2 inputMagnitude)
    {
        //Check Movement Speed
        if (_defaultInput.Character.Sprint.ReadValue<float>() > 0f && playerController.playerStance == Models.PlayerStance.Stand)
            amount = _initialAmount * (inputMagnitude.magnitude + 4) * GetSpeedMovement();
        else
            amount = _initialAmount * GetSpeedMovement();
    }

    private void UpdateHeadBob()
    {
        Vector2 inputMagnitude = playerController.InputMovement;
        SetAmountBySpeed(inputMagnitude);

        //Headbob when moving
        if (inputMagnitude.magnitude > 0)
            CalculateHeadBob();
    }

    private Vector3 CalculateHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, smooth * Time.deltaTime);
        transform.localPosition += pos;
        return pos;
    }

    private void StopHeadBob()
    {
        if (transform.localPosition == _startPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _startPos, 1 * Time.deltaTime);
    }

    private float GetSpeedMovement()
    {
        float speed = 0f;

        if (playerController.playerStance == PlayerStance.Stand)
        {
            if (playerController.IsSprinting)
                speed = 1f; //Sprint Speed
            else
                speed = 1f; //Walk Speed
        }
        else if (playerController.playerStance == PlayerStance.Crouch)
        {
            speed = 0; //Crouch Speed
        }
        else
        {
            speed = 0; //Prone Speed
        }

        if (playerController.CurrentSpeed == 0)
            speed = 0;

        return speed;
    }
}
