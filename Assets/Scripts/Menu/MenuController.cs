using UnityEngine;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void StartPauseCanState()
    {
        FindFirstObjectByType<BackToMenu>().GetComponent<InputState>().SetCanMouseState(false);
    }

    public void PlayButton()
    {
        LoadingData.instance.ChangeScene("Begin");
        FindFirstObjectByType<BackToMenu>().GetComponent<InputState>().SetCanMouseState(true);
    }
}
