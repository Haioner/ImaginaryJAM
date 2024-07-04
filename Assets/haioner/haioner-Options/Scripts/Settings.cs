using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using System.Collections.Generic;

[System.Serializable]
public class VideoSettings
{
    [Header("Quality")]
    public TMP_Dropdown QualityDropdown;
    [HideInInspector] public int CurrentQualityData;

    [Header("Display")]
    public TMP_Dropdown DisplayModeDropDown;
    [HideInInspector] public FullScreenMode CurrentScreenMode;
    [HideInInspector] public int CurrentDisplayModeData = 1;

    [Header("Resolution")]
    public Vector2[] Resolutions;
    public TMP_Dropdown ResolutionDropDown;
    [HideInInspector] public int CurrentResolutionData;

    [Header("RenderScale")]
    public UniversalRenderPipelineAsset UrpAsset;
    public TextMeshProUGUI RenderScaleText;
    public Slider RenderScaleSlider;
    [HideInInspector] public float CurrentRenderScaleData = 1f;

    [Header("Brightness")]
    public Slider BrightnessSlider;
    public TextMeshProUGUI BrightnessText;
    [HideInInspector] public float CurrentBrightnessData = 0f;

    [Header("PostProcessing")]
    public List<Volume> AllVolumes = new List<Volume>();
    public Toggle PostToggle;
    [HideInInspector] public int CurrentPostProcessingData;
}

[System.Serializable]
public class AudioSettings
{
    [Header("Audio Mixer")]
    public AudioMixer Mixer;

    [Header("Master Audio")]
    public Slider MasterSlider;
    public TextMeshProUGUI MasterText;
    [HideInInspector] public float CurrentMasterData;

    [Header("Sound Audio")]
    public Slider SoundSlider;
    public TextMeshProUGUI SoundText;
    [HideInInspector] public float CurrentSoundData;

    [Header("Music Audio")]
    public Slider MusicSlider;
    public TextMeshProUGUI MusicText;
    [HideInInspector] public float CurrentMusicData;
}

[System.Serializable]
public class GeralSettings
{
    [Header("Mouse Sensitivity")]
    public Slider SensitivitySlider;
    public TextMeshProUGUI SensitivityText;
    [HideInInspector] public float CurrentSensitivityData;
}
