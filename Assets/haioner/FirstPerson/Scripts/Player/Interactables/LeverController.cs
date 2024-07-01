using UnityEngine;

public class LeverController : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactMessage = "Interact";
    public string InteractMessage
    {
        get { return interactMessage; }
        set { interactMessage = value; }
    }

    [Space]
    [SerializeField] private DoorController door;
    private AudioSource audioSource;
    private Animator anim;
    private bool _leverActive = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        audioSource.Play();
        _leverActive = !_leverActive;
        anim.SetBool("Lever", _leverActive);

        if (door == null) return;
        door.SetDoorActive(true, door.InteractMessage);
        door.Interact();
        door.SetDoorActive(false, door.InteractMessage);
    }
}
