using System.Collections;
using UnityEngine;

public enum PasswordState
{
    Denied, Acess
}

public class PasswordController : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactMessage = "Interact";
    public string InteractMessage
    {
        get { return interactMessage; }
        set { interactMessage = value; }
    }
    private string initialInteractMessage;

    [Space]
    [SerializeField] private PasswordState passwordController;
    [SerializeField] private AudioClip deniedClip;

    private void Awake()
    {
        initialInteractMessage = interactMessage;
    }

    public void Interact()
    {
        if (passwordController == PasswordState.Acess)
        {

        }
        else
        {
            SoundManager.PlayAudioClip(deniedClip);
            StartCoroutine(UpdateInteractMessage());
        }
    }

    private IEnumerator UpdateInteractMessage()
    {
        interactMessage = "Dont know the password";
        yield return new WaitForSeconds(1);
        interactMessage = initialInteractMessage;
    }
}
