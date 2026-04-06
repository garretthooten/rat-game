using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource.volume = SettingsManager.instance.musicVolume;
        SettingsManager.instance.OnMusicVolumeChange += ChangeMusicVolume;
    }

    void OnDisable()
    {
        SettingsManager.instance.OnMusicVolumeChange -= ChangeMusicVolume;
    }

    private void ChangeMusicVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
