using UnityEngine;

public class LoadingData : MonoBehaviour
{
    public static LoadingData instance;
    public static string LoadingSceneName;

    public void ChangeScene(string sceneName)
    {
        LoadingSceneName = sceneName;
        TransitionController.instance.TransitionToSceneName("Loading");
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
