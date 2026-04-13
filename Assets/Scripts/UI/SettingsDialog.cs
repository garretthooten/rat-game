using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDialog : MonoBehaviour
{
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private TMP_Dropdown _musicOptionsDropdown;

    void Awake()
    {
        _sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        _fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        _musicOptionsDropdown.onValueChanged.AddListener(SetMusicOption);
    }

    void Start()
    {
        _sfxVolumeSlider.value = SettingsManager.instance.sfxVolume;
        _musicVolumeSlider.value = SettingsManager.instance.musicVolume;
        _musicOptionsDropdown.value = SettingsManager.instance.musicOption;
        _fullscreenToggle.isOn = SettingsManager.instance.fullscreen == 1 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSFXVolume(float value)
    {
        Debug.Log($"Setting SFX volume to {value}");
        SettingsManager.instance?.SetSFXVolume(value);
    }

    void SetMusicVolume(float value)
    {
        Debug.Log($"Setting music volume to {value}");
        SettingsManager.instance?.SetMusicVolume(value);
    }

    void SetFullscreen(bool value)
    {
        Debug.Log($"Setting fullscreen to {value}");
        //int boolValue = value ? 1 : 0;
        SettingsManager.instance?.SetFullscreen(value);
    }

    void SetMusicOption(int index)
    {
        Debug.Log($"Setting music option to {index}");
        SettingsManager.instance?.SetMusicOption(index);
    }
}
