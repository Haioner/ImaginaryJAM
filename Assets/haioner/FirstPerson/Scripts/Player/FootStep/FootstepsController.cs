using static Models;
using UnityEngine;

public class FootstepsController : MonoBehaviour
{
    [Header("FootsSteps")]
    [SerializeField] AudioSource footstepAudioSource;
    [SerializeField] private SO_StepsTags stepsTAGS;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;

    private PlayerController playerController;
    private Vector2 currentInput;
    private float footstepTimer = 0;
    private bool canLand;

    private void Awake() => playerController = GetComponentInParent<PlayerController>();

    private void Update()
    {
        if (playerController.playerStance == PlayerStance.Prone)
            return;

        HandleMovement();
        HandleLand();

        if (useFootsteps)
            Handle_Footsteps();
    }

    private float GetCurrentOffset()
    {
        if (playerController.playerStance == PlayerStance.Crouch)
            return baseStepSpeed * crouchStepMultiplier;

        else if (playerController.playerStance == PlayerStance.Stand && playerController.IsSprinting)
            return baseStepSpeed * sprintStepMultiplier;

        else
            return baseStepSpeed;
    }

    private void HandleLand()
    {
        if (playerController.IsGrounded)
        {
            if (canLand)
            {
                PlayAudios();
                canLand = false;
            }
        }
        else
            canLand = true;
    }

    private void HandleMovement()
    {
        currentInput = playerController.InputMovement;
    }

    private void Handle_Footsteps()
    {
        if (!playerController.IsGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            PlayAudios();
            footstepTimer = GetCurrentOffset();
        }
    }

    private void PlayAudios()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.3f))
        {
            bool matchingTag = false;

            foreach (var groundType in stepsTAGS.groundTypes)
            {
                if (hit.collider.tag == groundType.tagName)
                {
                    footstepAudioSource.PlayOneShot(groundType.clips[Random.Range(0, groundType.clips.Count - 1)]);
                    matchingTag = true;
                    break;
                }
            }

            if(!matchingTag)
                footstepAudioSource.PlayOneShot(stepsTAGS.groundTypes[0].clips[Random.Range(0, stepsTAGS.groundTypes[0].clips.Count - 1)]);
        }
    }
}
