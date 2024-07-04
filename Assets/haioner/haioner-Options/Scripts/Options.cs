using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    [Header("Video Settings")]
    [SerializeField] private VideoSettings videoSettings;

    [Header("Audio Settings")]
    [SerializeField] private AudioSettings audioSettings;

    [Header("Geral Settings")]
    [SerializeField] private GeralSettings geralSettings;

    #region Methods
    private void Awake()
    {
        LoadVideoSettings();
        LoadGeralSettings();
    }

    private void Start()
    {
        LoadAudioSettings();
    }
    #endregion

    #region Video

    public void ApplyVideoSettings()
    {
        ApplyQuality();
        ApplyDisplayMode();
        ApplyResolution();
        ApplyRenderScale();
        ApplyBrightness();
        ApplyPostProcessing();
    }

    private void LoadVideoSettings()
    {
        LoadQuality();
        LoadDisplay();
        LoadResolution();
        LoadRenderScale();
        LoadBrightness();
        LoadPostProcessing();
    }

    #region Quality

    public void SwichQuality()
    {
        videoSettings.CurrentQualityData = videoSettings.QualityDropdown.value;
    }

    private void ApplyQuality()
    {
        QualitySettings.SetQualityLevel(videoSettings.CurrentQualityData, true);
        SaveQuality();
    }

    private void SaveQuality()
    {
        PlayerPrefs.SetInt("data_quality", videoSettings.CurrentQualityData);
    }

    private void LoadQuality()
    {
        if (PlayerPrefs.HasKey("data_quality"))
        {
            videoSettings.CurrentQualityData = PlayerPrefs.GetInt("data_quality");
            videoSettings.QualityDropdown.SetValueWithoutNotify(videoSettings.CurrentQualityData);
        }
        else
        {
            videoSettings.CurrentQualityData = 5;
        }
        ApplyQuality();
    }

    #endregion

    #region DisplayMode
    private void ApplyDisplayMode()
    {
        ChangeScreenMode();
        SaveDisplay();
    }

    private void ChangeScreenMode()
    {
        videoSettings.CurrentDisplayModeData = videoSettings.DisplayModeDropDown.value;
        switch (videoSettings.CurrentDisplayModeData)
        {
            case 0:
                videoSettings.CurrentScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                videoSettings.CurrentScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                videoSettings.CurrentScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                videoSettings.CurrentScreenMode = FullScreenMode.Windowed;
                break;
        }
        Screen.fullScreenMode = videoSettings.CurrentScreenMode;
    }

    private void SaveDisplay()
    {
        PlayerPrefs.SetInt("display", videoSettings.CurrentDisplayModeData);
    }

    private void LoadDisplay()
    {
        if (PlayerPrefs.HasKey("display"))
        {
            videoSettings.CurrentDisplayModeData = PlayerPrefs.GetInt("display");
            videoSettings.DisplayModeDropDown.value = videoSettings.CurrentDisplayModeData;
            ApplyDisplayMode();
        }
    }
    #endregion

    #region Resolution
    private void CreateResolutionsSelections()
    {
        var resolutions = new List<string>();
        foreach (var resolution in videoSettings.Resolutions)
        {
            resolutions.Add(resolution.x.ToString() + " x " + resolution.y.ToString());
        }
        videoSettings.ResolutionDropDown.AddOptions(resolutions);
    }

    private void ApplyResolution()
    {
        int width = (int)videoSettings.Resolutions[videoSettings.ResolutionDropDown.value].x;
        int height = (int)videoSettings.Resolutions[videoSettings.ResolutionDropDown.value].y;
        Screen.SetResolution(width, height, videoSettings.CurrentScreenMode);
        SaveResolution();
    }

    private void SaveResolution()
    {
        videoSettings.CurrentResolutionData = videoSettings.ResolutionDropDown.value;
        PlayerPrefs.SetInt("resolution", videoSettings.CurrentResolutionData);
    }

    private void LoadResolution()
    {
        CreateResolutionsSelections();

        if (PlayerPrefs.HasKey("resolution"))
        {
            videoSettings.CurrentResolutionData = PlayerPrefs.GetInt("resolution");
            videoSettings.ResolutionDropDown.value = videoSettings.CurrentResolutionData;
        }

        ApplyResolution();
    }
    #endregion

    #region RenderScale
    public void RenderScaleValue(float sliderValue)
    {
        videoSettings.CurrentRenderScaleData = sliderValue;
        videoSettings.RenderScaleText.text = sliderValue.ToString("F2");
    }

    private void ApplyRenderScale()
    {
        videoSettings.UrpAsset.renderScale = videoSettings.CurrentRenderScaleData;
        SaveRenderScale();
    }

    private void SaveRenderScale()
    {
        PlayerPrefs.SetFloat("renderScale", videoSettings.CurrentRenderScaleData);
    }

    private void LoadRenderScale()
    {
        if (PlayerPrefs.HasKey("renderScale"))
        {
            videoSettings.CurrentRenderScaleData = PlayerPrefs.GetFloat("renderScale");
        }

        videoSettings.RenderScaleSlider.SetValueWithoutNotify(videoSettings.CurrentRenderScaleData);
        videoSettings.RenderScaleText.text = videoSettings.CurrentRenderScaleData.ToString("F2");
        ApplyRenderScale();
    }
    #endregion

    #region Brightnees

    public void BrightnessValue(float sliderValue)
    {
        videoSettings.CurrentBrightnessData = sliderValue;
        videoSettings.BrightnessText.text = sliderValue.ToString("F2");
    }

    private void ApplyBrightness()
    {
        ColorAdjustments coloradj;
        for (int i = 0; i < videoSettings.AllVolumes.Count; i++)
        {
            if (videoSettings.AllVolumes[i].sharedProfile.TryGet<ColorAdjustments>(out coloradj))
            {
                coloradj.postExposure.value = videoSettings.CurrentBrightnessData;
            }
        }
        
        SaveBrightness();
    }

    private void SaveBrightness()
    {
        PlayerPrefs.SetFloat("brightness", videoSettings.CurrentBrightnessData);
    }

    private void LoadBrightness()
    {
        if (PlayerPrefs.HasKey("brightness"))
        {
            videoSettings.CurrentBrightnessData = PlayerPrefs.GetFloat("brightness");
        }

        videoSettings.BrightnessSlider.SetValueWithoutNotify(videoSettings.CurrentBrightnessData);
        videoSettings.BrightnessText.text = videoSettings.CurrentBrightnessData.ToString("F2");
        BrightnessValue(videoSettings.CurrentBrightnessData);
    }
    #endregion

    #region PostProcessing

    public void SwichPostProcessing()
    {
        if (videoSettings.CurrentPostProcessingData == 0)
            videoSettings.CurrentPostProcessingData = 1;
        else
            videoSettings.CurrentPostProcessingData = 0;
    }

    private void ApplyPostProcessing()
    {
        //Find all post loaders in post volumes
        PostProcessingLoader[] allPostLoaders = FindObjectsOfType<PostProcessingLoader>();
        foreach (PostProcessingLoader item in allPostLoaders)
        {
            item.SetPostState(videoSettings.PostToggle.isOn);
        }

        //Prevent to change brightness
        if (videoSettings.CurrentPostProcessingData == 0)
            videoSettings.BrightnessSlider.interactable = false;
        else
            videoSettings.BrightnessSlider.interactable = true;

        SavePostProcessing();
    }

    private void SavePostProcessing()
    {
        PlayerPrefs.SetInt("postprocessing", videoSettings.CurrentPostProcessingData);
    }

    private void LoadPostProcessing()
    {
        if (PlayerPrefs.HasKey("postprocessing"))
        {
            videoSettings.CurrentPostProcessingData = PlayerPrefs.GetInt("postprocessing");
            if (videoSettings.CurrentPostProcessingData == 0)
                videoSettings.PostToggle.SetIsOnWithoutNotify(false);
            else
                videoSettings.PostToggle.SetIsOnWithoutNotify(true);
        }
        else
        {
            videoSettings.CurrentPostProcessingData = 1;
        }
        ApplyPostProcessing();
    }
    #endregion

    #endregion

    #region Audio

    private void LoadAudioSettings()
    {
        LoadMasterVolume();
        LoadSoundVolume();
        LoadMusicVolume();
    }

    private float AudioValueText(float value)
    {
        return value * 100;
    }

    #region Master
    public void MasterVolume()
    {
        audioSettings.CurrentMasterData = audioSettings.MasterSlider.value;
        audioSettings.Mixer.SetFloat("MasterVolume", Mathf.Log10(audioSettings.MasterSlider.value) * 20);
        PlayerPrefs.SetFloat("MVolume", audioSettings.MasterSlider.value);
        MasterText();
    }

    private void MasterText()
    {
        audioSettings.MasterText.text = AudioValueText(audioSettings.CurrentMasterData).ToString("F0");
    }

    private void LoadMasterVolume()
    {
        if (PlayerPrefs.HasKey("MVolume"))
        {
            audioSettings.CurrentMasterData = PlayerPrefs.GetFloat("MVolume");
            audioSettings.MasterSlider.SetValueWithoutNotify(audioSettings.CurrentMasterData);
        }
        MasterVolume();
    }
    #endregion

    #region Sound
    public void SoundVolume()
    {
        audioSettings.CurrentSoundData = audioSettings.SoundSlider.value;
        audioSettings.Mixer.SetFloat("SoundVolume", Mathf.Log10(audioSettings.SoundSlider.value) * 20);
        PlayerPrefs.SetFloat("SVolume", audioSettings.SoundSlider.value);
        SoundText();
    }

    private void SoundText()
    {
        audioSettings.SoundText.text = AudioValueText(audioSettings.CurrentSoundData).ToString("F0");
    }

    private void LoadSoundVolume()
    {
        if (PlayerPrefs.HasKey("SVolume"))
        {
            audioSettings.CurrentSoundData = PlayerPrefs.GetFloat("SVolume");
            audioSettings.SoundSlider.SetValueWithoutNotify(audioSettings.CurrentSoundData);
        }
        SoundVolume();
    }
    #endregion

    #region Music
    public void MusicVolume()
    {
        audioSettings.CurrentMusicData = audioSettings.MusicSlider.value;
        audioSettings.Mixer.SetFloat("MusicVolume", Mathf.Log10(audioSettings.MusicSlider.value) * 20);
        PlayerPrefs.SetFloat("MuVolume", audioSettings.MusicSlider.value);
        MusicText();
    }

    private void MusicText()
    {
        audioSettings.MusicText.text = AudioValueText(audioSettings.CurrentMusicData).ToString("F0");
    }

    private void LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey("MuVolume"))
        {
            audioSettings.CurrentMusicData = PlayerPrefs.GetFloat("MuVolume");
            audioSettings.MusicSlider.SetValueWithoutNotify(audioSettings.CurrentMusicData);
        }
        MusicVolume();
    }
    #endregion

    #endregion

    #region Geral

    private void LoadGeralSettings()
    {
        LoadSensitivity();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0); //Menu Scene
    }

    #region Sensitivity
    public void MouseSensitivity()
    {
        geralSettings.CurrentSensitivityData = geralSettings.SensitivitySlider.value;
        PlayerPrefs.SetFloat("Sensitivity", geralSettings.CurrentSensitivityData);
        SetSensitivity();
        SensitivityText();
    }

    private void SetSensitivity()
    {
        if(FindObjectOfType<MouseSensitivityManager>() != null)
            FindObjectOfType<MouseSensitivityManager>().SetSensitivity(geralSettings.CurrentSensitivityData);
    }

    private void SensitivityText()
    {
        geralSettings.SensitivityText.text = geralSettings.CurrentSensitivityData.ToString("F0");
    }

    private void LoadSensitivity()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            geralSettings.CurrentSensitivityData = PlayerPrefs.GetFloat("Sensitivity");
            geralSettings.SensitivitySlider.SetValueWithoutNotify(geralSettings.CurrentSensitivityData);
        }
        MouseSensitivity();
    }
    #endregion

    #endregion
}
