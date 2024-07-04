using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public void LoadingChangeScene(string sceneName)
    {
        LoadingData.instance.ChangeScene(sceneName);
    }
}
