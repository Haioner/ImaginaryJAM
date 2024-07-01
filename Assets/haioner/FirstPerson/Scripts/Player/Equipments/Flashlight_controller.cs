using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Flashlight_controller : MonoBehaviour
{
    [Header("FlashLihgt")]
    [SerializeField] private List<Light> lights = new List<Light>();
    [SerializeField] private AudioSource flashAudio;
    private DefaultInput _defaultInput;
    private List<float> initialIntensities = new List<float>();
    private bool _isFlashing;
    private bool _canLight = true;

    [Header("Flicker")]
    [SerializeField] private float flickerDuration = 0.05f;
    [SerializeField] private float flickerDelay = 10f;
    [SerializeField] private Vector2 minMaxflickerCount = new Vector2(5, 15);
    private bool _flickerActive = false;
    private float _flickerTimer = 0f;
    private int _flickerCount = 15;

    [Header("Battery")]
    [SerializeField] private bool canBattery;
    [Range(0, 5)][SerializeField] private int batteryValue = 5;
    [SerializeField] private float batteryWasteVelocity = 3f;
    [SerializeField] private Slider batterySlider;
    private float _currentBatteryVelocity = 100f;

    private void Awake()
    {
        batterySlider.maxValue = batteryValue;

        _defaultInput = new DefaultInput();
        _defaultInput.Flashlight.LightSwitch.performed += e => SwitchLight();
        _defaultInput.Enable();

        for (int i = 0; i < lights.Count; i++)
        {
            initialIntensities.Add(lights[i].intensity);
        }

        _isFlashing = lights[0].enabled;
    }

    void Update()
    {
        FlickFlashLight();
        CalculateBattery();
    }

    void FlickFlashLight()
    {
        if (_flickerActive)
        {
            _flickerTimer += Time.deltaTime;

            if (_flickerTimer >= flickerDuration)
            {
                for (int i = 0; i < lights.Count; i++)
                    lights[i].intensity = Random.Range(0.01f, 0.2f);
                

                _flickerTimer = 0f;
                _flickerCount--;

                if (_flickerCount <= 0)
                {
                    for (int i = 0; i < lights.Count; i++)
                        lights[i].intensity = initialIntensities[i];

                    _flickerActive = false;
                    _flickerTimer = 0f;
                }
            }
        }
        else
        {
            _flickerTimer += Time.deltaTime;

            if (_flickerTimer >= flickerDelay)
            {
                for (int i = 0; i < lights.Count; i++)
                    lights[i].intensity = initialIntensities[i];
                _flickerActive = true;
                _flickerCount = Random.Range((int)minMaxflickerCount.x, (int)minMaxflickerCount.y);
                _flickerTimer = 0f;
            }
        }

    }

    private void SwitchLight()
    {
        if (!gameObject.activeInHierarchy || !_canLight) return;
        flashAudio.Play();

        _isFlashing = !_isFlashing;
        for (int i = 0; i < lights.Count; i++)
            lights[i].enabled = !lights[i].enabled;
    }

    public bool AddBattery(int value)
    {
        if (batteryValue < 5)
        {
            _canLight = true;
            batteryValue += value;
            return true;
        }
        else
            return false;
    }

    private void CalculateBattery()
    {
        if (!canBattery) return;

        if (_isFlashing && batteryValue > 0)
        {
            _currentBatteryVelocity -= Time.deltaTime * batteryWasteVelocity;
        }

        if(_currentBatteryVelocity <= 0)
        {
            _currentBatteryVelocity = 100f;
            batteryValue--;
        }

        if(batteryValue <= 0)
        {
            SwitchLight();
            _canLight = false;
        }

        UpdateBatterySlider();
    }

    private void UpdateBatterySlider()
    {
        batterySlider.value = batteryValue;
        BatterySliderColor();
    }

    private void BatterySliderColor()
    {
        ColorBlock colors = batterySlider.colors;
        if (batteryValue <= 1)
        {
            colors.disabledColor = Color.red;
        }
        else
        {
            colors.disabledColor = Color.white;
        }
        batterySlider.colors = colors;
    }
}
