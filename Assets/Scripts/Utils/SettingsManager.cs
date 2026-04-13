using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance { private set; get; }
    public float sfxVolume;
    public float musicVolume;
    public int musicOption;
    public int fullscreen;

    public event Action<float> OnSFXVolumeChange;
    public event Action<float> OnMusicVolumeChange;
    public event Action<int> OnMusicOptionChange;
    //public event Action OnFullscreenChange;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
        
        if (PlayerPrefs.GetInt("Initialized") != 1)
        {
            Debug.Log("Initializing playerprefs");
            // no playerprefs, must be first boot. create init values
            PlayerPrefs.SetFloat("SFXVolume", 1.0f);
            PlayerPrefs.SetFloat("MusicVolume", 1.0f);
            PlayerPrefs.SetInt("Fullscreen", 0);
            PlayerPrefs.SetInt("MusicOption", 0);
            PlayerPrefs.SetInt("Initialized", 1);
        }
        Debug.Log("Reading playerprefs");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        musicOption = PlayerPrefs.GetInt("MusicOption");
        fullscreen = PlayerPrefs.GetInt("Fullscreen");
        Screen.fullScreen = fullscreen == 1 ? true : false;
        
        DontDestroyOnLoad(gameObject);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        OnSFXVolumeChange?.Invoke(volume);
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        OnMusicVolumeChange?.Invoke(volume);
    }

    public void SetMusicOption(int option)
    {
        musicOption = option;
        PlayerPrefs.SetInt("MusicOption", option);
        OnMusicOptionChange?.Invoke(option);
    }

    public void SetFullscreen(bool value)
    {
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
        Screen.fullScreen = value;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
