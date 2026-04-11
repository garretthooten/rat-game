using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager instance { get; private set; }
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _musicClips;
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource.volume = SettingsManager.instance.musicVolume;
        _audioSource.clip = _musicClips[0];
        SettingsManager.instance.OnMusicVolumeChange += ChangeMusicVolume;
        _audioSource.Play();
    }

    void OnDisable()
    {
        SettingsManager.instance.OnMusicVolumeChange -= ChangeMusicVolume;
    }

    private void ChangeMusicVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public void ChangeMusicOption(int index)
    {
        _audioSource.Stop();
        _audioSource.clip = _musicClips[index];
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
