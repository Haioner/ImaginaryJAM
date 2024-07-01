using System.Collections;
using UnityEngine;

public class FlashLightBattery : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactMessage = "Interact";
    public string InteractMessage
    {
        get { return interactMessage; }
        set { interactMessage = value; }
    }
    private string initialInteractMessage;

    [Space]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int batteryCharge = 5;
    private bool _canInteract = true;

    private void Awake() => initialInteractMessage = interactMessage;

    public void Interact()
    {
        if (!_canInteract) return;
        Flashlight_controller flashLight = FindAnyObjectByType<Flashlight_controller>();
        if (flashLight.AddBattery(batteryCharge))
        {
            audioSource.Play();
            _canInteract = false;
            StartCoroutine(DestroyOnAudioEnd());
        }
        else
        {
            StartCoroutine(UpdateInteractMessage());
        }
    }

    private IEnumerator UpdateInteractMessage()
    {
        interactMessage = "Full Battery";
        yield return new WaitForSeconds(1);
        interactMessage = initialInteractMessage;
    }

    private IEnumerator DestroyOnAudioEnd()
    {
        while (audioSource.isPlaying)
            yield return null;

        Destroy(gameObject);
    }
}
