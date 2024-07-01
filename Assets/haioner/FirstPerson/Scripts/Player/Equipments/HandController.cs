using static Models;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("CACHE")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator anim;

    private bool _isGroundedTrigger;
    private float _fallingDelay;
    private DefaultInput _defaultInput;

    private void Awake()
    {
        _defaultInput = new DefaultInput();
        _defaultInput.Character.Jump.performed += e => TriggerJump();
        _defaultInput.Enable();
    }

    private void Update() => SetHandAnimations();

    private void TriggerJump()
    {
        if (!playerController.IsGrounded) return;

        _isGroundedTrigger = false;
        anim.SetTrigger("Jump");
    }

    private void SetHandAnimations()
    {
        if (_isGroundedTrigger)
            _fallingDelay = 0;
        else
            _fallingDelay += Time.deltaTime;

        if (playerController.IsGrounded && !_isGroundedTrigger && _fallingDelay > 0.1f)
        {
            anim.SetTrigger("Land");
            _isGroundedTrigger = true;
        }
        else if (!playerController.IsGrounded && _isGroundedTrigger)
        {
            anim.SetTrigger("Falling");
            _isGroundedTrigger = false;
        }

        if (playerController.playerStance == PlayerStance.Stand)
            anim.SetBool("isSprinting", playerController.IsSprinting);

        anim.SetFloat("AnimationSpeed", SpeedMovement());
    }

    private float SpeedMovement()
    {
        float speed = 0f;

        if(playerController.playerStance == PlayerStance.Stand)
        {
            if (playerController.IsSprinting)
                speed = 1.1f; //Sprint Speed
            else
                speed = 1f; //Walk Speed
        }
        else if(playerController.playerStance == PlayerStance.Crouch)
        {
            speed = 0.75f; //Crouch Speed
        }
        else
        {
            speed = 0.5f; //Prone Speed
        }

        if (playerController.CurrentSpeed == 0)
            speed = 0;

        return speed;
    }
}
