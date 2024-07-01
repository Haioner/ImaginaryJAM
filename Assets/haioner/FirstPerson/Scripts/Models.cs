using UnityEngine;

public static class Models
{
    #region Player

    [System.Serializable]
    public class ViewSettings
    {
        [Header("View Settings")]
        public bool CanView = true;
        public float ViewXSensitivity = 10f;
        public float ViewYSensitivity = 10f;

        [Space]
        public bool ViewXInverted = false;
        public bool ViewYInverted = false;

        [Space]
        public float viewClampVerticalMin = -70f;
        public float viewClampVerticalMax = 60f;

        [Space]
        public bool CanClampHorizontal = false;
        public float viewClampHorizontalMin = -90f;
        public float viewClampHorizontalMax = 90f;
    }

    [System.Serializable]
    public class MovementSettings
    {
        [Header("Movement")]
        public bool CanMove = true;
        public float MovementSmoothing = 0.15f;
        public float SmoothInputSpeed = 0.2f;

        [Header("Walking")]
        public float WalkingForwardSpeed = 4f;
        public float WalkingStrafeSpeed = 4f;
        public float WalkingBackwardSpeed = 2f;

        [Header("Running")]
        public float RunningForwardSpeed = 7f;
        public float RunningStrafeSpeed = 7f;
        public float FovToAdd = 15f;
        public float FovVelocity = 5f;

        [Header("Run-Stamina")]
        public bool CanStamina = true;
        public float MaxStamina = 100f;
        public float MinCanRunAfterZero = 15f;
        public float StaminaWasteVelocity = 10f;
        public float StaminaChargeVelocity = 5f;
        public AudioSource PantingAudioSource;

        [Header("Speed Effectors")]
        public float CrouchSpeedEffector = 0.6f;
        public float ProneSpeedEffector = 0.2f;
        public float FallingSpeedEffector = 1f;
    }

    [System.Serializable]
    public class GravitySettings
    {
        [Header("Gravity")]
        public float GravityAmount = 0.3f;
        public float GravityMin = -3f;

        [Header("Jump")]
        public float JumpingHeight = 6f;
        public float JumpingFallof = 1.5f;
        public float FallingSmoothing = 0.1f;
    }

    [System.Serializable]
    public class CharacterStance
    {
        public float CameraHeight;
        public CapsuleCollider StanceCollider;
    }

    public enum PlayerStance
    {
        Stand,
        Crouch,
        Prone,
        Seated
    }

    #endregion

    #region Equipments

    [System.Serializable]
    public class SwaySettings
    {
        [Header("Equipment Sway")]
        public float SwayAmount = 10f;
        public float SwaySmoothing = 0.05f;
        public float SwayResetSmoothing = 0.05f;

        [Space]
        public bool SwayYInverted = true;
        public bool SwayXInverted = true;

        [Space]
        public float SwayClampX = 5f;
        public float SwayClampY = 5f;
    }

    [System.Serializable]
    public class LeaningMovementSettings
    {
        [Header("Movement Leaning")]
        public float AmountHorizontal = 2f;
        public float AmountVertical = 1f;
        public float MovementLeaningSmoothing = 0.3f;

        [Space]
        public bool HorizontalInverted = true;
        public bool VerticalInverted = false;
    }

    #endregion
}
