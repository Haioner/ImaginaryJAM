using static Models;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator anim;

    private float _smoothInputX;
    private float _smoothInputY;
    private float _smoothTime = 0.2f;

    private DefaultInput _defaultInput;

    private void Awake()
    {
        _defaultInput = new DefaultInput();
        _defaultInput.Character.Jump.performed += e => JumpAnimation();
        _defaultInput.Enable();
    }

    void Update()
    {
        UpdateSmoothInput();
        MovementAnimations();
        UpdateStancesAnimations();
    }

    private void UpdateStancesAnimations()
    {
        if(playerController.playerStance == PlayerStance.Stand)
            EnableJustOneBool("isStand");
        else if(playerController.playerStance == PlayerStance.Crouch)
            EnableJustOneBool("isCrouch");
        else if (playerController.playerStance == PlayerStance.Prone)
            EnableJustOneBool("isProne");
        else if (playerController.playerStance == PlayerStance.Seated)
            EnableJustOneBool("isSeated");
        else
            EnableJustOneBool("isStand");
    }

    private void EnableJustOneBool(string boolName)
    {
        anim.SetBool("isStand", false);
        anim.SetBool("isCrouch", false);
        anim.SetBool("isProne", false);
        anim.SetBool("isSeated", false);

        anim.SetBool(boolName, true);
    }

    private void UpdateSmoothInput()
    {
        if (playerController.IsSprinting)
        {
            _smoothInputX = Mathf.Lerp(_smoothInputX, playerController.currentInputVector.x, Time.deltaTime / _smoothTime);
            _smoothInputY = Mathf.Lerp(_smoothInputY, playerController.currentInputVector.y, Time.deltaTime / _smoothTime);
        }
        else
        {
            _smoothInputX = Mathf.Lerp(_smoothInputX, playerController.currentInputVector.x / 2, Time.deltaTime / _smoothTime);
            _smoothInputY = Mathf.Lerp(_smoothInputY, playerController.currentInputVector.y / 2, Time.deltaTime / _smoothTime);
        }
    }

    private void MovementAnimations()
    {
        anim.SetFloat("inputX", _smoothInputX);
        anim.SetFloat("inputY", _smoothInputY);
    }

    private void JumpAnimation()
    {
        if (playerController.IsGrounded && playerController.playerStance == PlayerStance.Stand)
            anim.SetTrigger("Jump");
    }
}
