using UnityEngine;

public class MouseSensitivityManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    public void SetSensitivity(float sensitivityValue)
    {
        playerController.SetSensitivity(sensitivityValue);
    }
}
