using UnityEngine;

public class LockerInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactMessage = "Interact";
    public string InteractMessage
    {
        get { return interactMessage; }
        set { interactMessage = value; }
    }

    [SerializeField] private DoorController doorController;
    [SerializeField] private AudioClip interactClip;

    private Animator anim;
    private bool isPressed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetDoorController(DoorController newDoor)
    {
        doorController = newDoor;

        if (isPressed)
        {
            doorController.ForceOpen();
        }
    }

    public virtual void Interact()
    {
        isPressed = !isPressed;

        anim.SetBool("leverOn", isPressed);
        SoundManager.PlayAudioClip(interactClip);
        GetComponent<InteractEvent>().CallInteract();

        if(doorController != null)
        {
            doorController.SetDoorActive(true, doorController.InteractMessage);
            doorController.Interact();
            doorController.SetDoorActive(false, doorController.InteractMessage);
        }
    }

}
