using static Models;
using UnityEngine;

public class Leaning : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] LeaningMovementSettings leaningSettings;

    private Vector3 _initialRotation;

    private Vector3 _targetMovementRotation;
    private Vector3 _targetMovementRotationVelocity;

    private Vector3 _newMovementRotation;
    private Vector3 _newMovementRotationVelocity;

    private void Awake() => _initialRotation = transform.localRotation.eulerAngles;
    void FixedUpdate() => CalculateLeaning();

    private void CalculateLeaning()
    {
        //Get Input Z && X
        _targetMovementRotation.z =
            leaningSettings.AmountHorizontal * (leaningSettings.HorizontalInverted ? -playerController.InputMovement.x : playerController.InputMovement.x);

        _targetMovementRotation.x =
            leaningSettings.AmountVertical * (leaningSettings.VerticalInverted ? -playerController.InputMovement.y : playerController.InputMovement.y);

        //Set Rotations
        _targetMovementRotation =
            Vector3.SmoothDamp(_targetMovementRotation, Vector3.zero, ref _targetMovementRotationVelocity, leaningSettings.MovementLeaningSmoothing);
        _newMovementRotation =
            Vector3.SmoothDamp(_newMovementRotation, _targetMovementRotation, ref _newMovementRotationVelocity, leaningSettings.MovementLeaningSmoothing);

        //Apply
        transform.localRotation = Quaternion.Euler(_initialRotation + _newMovementRotation);
    }
}
