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
    public bool canGunConnect = true;

    [Header("Renderer")]
    [SerializeField] private MeshRenderer lockerMeshRenderer;
    [SerializeField] private Material[] materials;

    private Animator anim;
    private bool _doorOpened = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeInitialMat(Material material)
    {
        if (materials.Length > 0)
        {
            materials[0] = material;
        }
    }

    public void SetMaterial(int matIndex)
    {
        lockerMeshRenderer.material = materials[matIndex];
    }

    public void SetDoorActive(bool state, string _interactMessge)
    {
        canInteract = state;
        interactMessage = _interactMessge;

        if (TryGetComponent(out InteractEvent interactEvent) && state)
            interactEvent.CallInteract();
    }

    public void ForceState(bool state)
    {
        _doorOpened = state;
        if (!state)
            anim.Play("Closed");

        anim.SetBool("Door", _doorOpened);

        PlaySounds();

        if (TryGetComponent(out InteractEvent interactEvent) && state)
            interactEvent.CallInteract();
    }

    public void Interact()
    {
        PlaySounds();

        if (canInteract)
        {
            DoorAnimations();

            if (TryGetComponent(out InteractEvent interactEvent))
                interactEvent.CallInteract();
        }
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
