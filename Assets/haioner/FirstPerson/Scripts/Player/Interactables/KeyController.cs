using System.Collections;
using UnityEngine;

public class KeyController : MonoBehaviour, IInteractable
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

    private void Awake() => audioSource = GetComponent<AudioSource>();

    public void Interact()
    {
        door.SetDoorActive(true, "Interact");
        audioSource.Play();
        StartCoroutine(DestroyOnAudioEnd());
    }

    private IEnumerator DestroyOnAudioEnd()
    {
        while (audioSource.isPlaying)
            yield return null;
        
        Destroy(gameObject);
    }
}
