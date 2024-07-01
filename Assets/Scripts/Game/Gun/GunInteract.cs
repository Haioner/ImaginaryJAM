using UnityEngine;

public class GunInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactMessage = "Interact";
    public string InteractMessage
    {
        get { return interactMessage; }
        set { interactMessage = value; }
    }

    [SerializeField] private GameObject playerGun;

    public void Interact()
    {
        playerGun.SetActive(true);
        Destroy(gameObject);
    }
}
