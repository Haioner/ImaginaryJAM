using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Door Interaction")]
    [SerializeField] private bool canInteract = true;
    [SerializeField] private string interactMessage = "Interact";
    public string InteractMessage
    {
        get { return interactMessage; }
        set { interactMessage = value; }
    }

    [Header("Door Sounds")]
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip lockedClip;
    [SerializeField] private AudioClip closeClip;
    [SerializeField] private float delayCloseSound;

    [Header("Renderer")]
    [SerializeField] private MeshRenderer lockerMeshRenderer;
    [SerializeField] private Material[] materials;

    private Animator anim;
    private bool _doorOpened = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetMaterial(int matIndex)
    {
        lockerMeshRenderer.material = materials[matIndex];
    }

    public void SetDoorActive(bool state, string _interactMessge)
    {
        canInteract = state;
        interactMessage = _interactMessge;
    }

    public void ForceState(bool state)
    {
        _doorOpened = state;
        anim.SetBool("Door", _doorOpened);
        PlaySounds();
    }

    public void Interact()
    {
        PlaySounds();

        if (canInteract)
            DoorAnimations();
    }

    private void DoorAnimations()
    {
        _doorOpened = !_doorOpened;
        anim.SetBool("Door", _doorOpened);
    }

    private void PlaySounds()
    {
        if (openClip == null) return;
        if (!canInteract)
        {
            SoundManager.PlayAudioClip(lockedClip);
            return;
        }

        if (_doorOpened)
        {
            //_audioSource.clip = closeClip;
            //_audioSource.PlayDelayed(delayCloseSound);
            SoundManager.PlayAudioClip(closeClip);
        }
        else
        {
            SoundManager.PlayAudioClip(openClip);
        }
    }
}
