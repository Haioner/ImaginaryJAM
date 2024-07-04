using UnityEngine.Rendering;
using UnityEngine;

public class PostProcessingLoader : MonoBehaviour
{
    //How to use: Put in the volume with post component
    [SerializeField] private Volume m_Volume;
    private int data_PostProcessing = 1;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("postprocessing"))
            data_PostProcessing = PlayerPrefs.GetInt("postprocessing");
        else
            data_PostProcessing = 1;

        if (data_PostProcessing == 1)
            SetPostState(true);
        else
            SetPostState(false);
    }

    public void SetPostState(bool state)
    {
        m_Volume.enabled = state;
    }
}
