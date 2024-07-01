using static Models;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [Header("CACHE")]
    [SerializeField] private PlayerController playerController;

    [Header("Settings")]
    [SerializeField] SwaySettings swaySettings;

    private Vector3 _newRotation;
    private Vector3 _newRotationVelocity;

    private Vector3 _targetRotation;
    private Vector3 _targetRotationVelocity;

    private Vector3 _newMovementRotation;
    private Vector3 _newMovementRotationVelocity;

    private Vector3 _targetMovementRotation;
    private Vector3 _targetMovementRotationVelocity;

    private void Start() => _newRotation = transform.localRotation.eulerAngles;
    private void Update() => CalculateSway();

    private void CalculateSway()
    {
        _targetRotation.y += swaySettings.SwayAmount * (swaySettings.SwayXInverted ? -playerController.InputView.x : playerController.InputView.x) * Time.deltaTime;
        _targetRotation.x += swaySettings.SwayAmount * (swaySettings.SwayYInverted ? playerController.InputView.y : -playerController.InputView.y) * Time.deltaTime;

        _targetRotation.x = Mathf.Clamp(_targetRotation.x, -swaySettings.SwayClampX, swaySettings.SwayClampX);
        _targetRotation.y = Mathf.Clamp(_targetRotation.y, -swaySettings.SwayClampY, swaySettings.SwayClampY);
        _targetRotation.z = _targetRotation.y;

        _targetRotation = Vector3.SmoothDamp(_targetRotation, Vector3.zero, ref _targetRotationVelocity, swaySettings.SwayResetSmoothing);
        _newRotation = Vector3.SmoothDamp(_newRotation, _targetRotation, ref _newRotationVelocity, swaySettings.SwaySmoothing);

        _targetMovementRotation =
            Vector3.SmoothDamp(_targetMovementRotation, Vector3.zero, ref _targetMovementRotationVelocity, swaySettings.SwaySmoothing);
        _newMovementRotation =
            Vector3.SmoothDamp(_newMovementRotation, _targetMovementRotation, ref _newMovementRotationVelocity, swaySettings.SwaySmoothing);

        transform.localRotation = Quaternion.Euler(_newRotation + _newMovementRotation);
    }
}
