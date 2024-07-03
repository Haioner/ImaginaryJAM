using UnityEngine;
using System.Collections.Generic;

public class LockerInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactMessage = "Interact";
    public string InteractMessage
    {
        get { return interactMessage; }
        set { interactMessage = value; }
    }

    public bool canInteract = true;
    [SerializeField] private DoorController doorController;
    [SerializeField] private AudioClip interactClip;
    [SerializeField] private float clipVolume = 1f;
    public bool canGunConnect = true;
    [SerializeField] private bool isPressed;

    [Header("Renderer")]
    [SerializeField] private MeshRenderer[] lockerMeshRenderer;
    [SerializeField] private Material[] materials;

    private Animator anim;
    private int collidersCount;
    private Dictionary<Collider, bool> colliders = new Dictionary<Collider, bool>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        List<Collider> toRemove = new List<Collider>();

        foreach (var collider in colliders.Keys)
        {
            if (collider == null)
            {
                toRemove.Add(collider);
            }
        }

        foreach (var collider in toRemove)
        {
            colliders.Remove(collider);
            collidersCount--;
            if (collidersCount <= 0)
                ButtonCollision(false);
        }
    }

    public void SetMaterial(int matIndex)
    {
        foreach (var item in lockerMeshRenderer)
        {
            item.material = materials[matIndex];
        }
    }

    public void SetDoorController(DoorController newDoor)
    {
        doorController = newDoor;
        if (doorController != null)
        {
            doorController.ForceState(isPressed);
        }
    }

    public virtual void Interact()
    {
        if (!canInteract) return;

        isPressed = !isPressed;
        if (doorController != null)
        {
            doorController.SetDoorActive(true, doorController.InteractMessage);
            doorController.Interact();
            doorController.SetDoorActive(false, doorController.InteractMessage);
        }

        UpdateInteractFeedback();
    }

    public void ButtonInteract()
    {
        if (isPressed)
        {
            isPressed = !isPressed;
            anim.SetBool("isPressed", isPressed);

            if (doorController != null)
            {
                doorController.SetDoorActive(true, doorController.InteractMessage);
                doorController.Interact();
                doorController.SetDoorActive(false, doorController.InteractMessage);
            }
        }
    }

    private void UpdateInteractFeedback()
    {
        anim.SetBool("isPressed", isPressed);

        if (interactClip != null)
            SoundManager.PlayAudioClipVolume(interactClip, clipVolume);

        if (TryGetComponent(out InteractEvent interactEvent))
            interactEvent.CallInteract();
    }

    private void ButtonCollision(bool state)
    {
        isPressed = state;
        SetDoorController(doorController);
        UpdateInteractFeedback();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canInteract) return;

        if (!colliders.ContainsKey(collision.collider))
        {
            colliders.Add(collision.collider, true);
            collidersCount++;
            ButtonCollision(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (canInteract) return;

        if (colliders.ContainsKey(collision.collider))
        {
            colliders.Remove(collision.collider);
            collidersCount--;
            if (collidersCount <= 0)
                ButtonCollision(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canInteract) return;

        if ((other.CompareTag("Player") || other.CompareTag("Metic")) && !colliders.ContainsKey(other))
        {
            colliders.Add(other, true);
            collidersCount++;
            ButtonCollision(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (canInteract) return;

        if ((other.CompareTag("Player") || other.CompareTag("Metic")) && colliders.ContainsKey(other))
        {
            colliders.Remove(other);
            collidersCount--;
            if (collidersCount <= 0)
                ButtonCollision(false);
        }
    }
}
