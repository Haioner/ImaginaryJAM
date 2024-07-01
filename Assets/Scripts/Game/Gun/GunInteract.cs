using TMPro;
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
    [SerializeField] private TextMeshProUGUI tutorialGunText;

    public void Interact()
    {
        playerGun.SetActive(true);
        tutorialGunText.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
