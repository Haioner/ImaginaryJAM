using UnityEngine;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
