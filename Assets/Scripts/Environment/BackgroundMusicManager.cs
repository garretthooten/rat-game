using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager instance { get; private set; }
    private AudioSource _audioSource;
    [SerializeField] private int selectedIndex;
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
        _audioSource.clip = _musicClips[SettingsManager.instance.musicOption];
        SettingsManager.instance.OnMusicVolumeChange += ChangeMusicVolume;
        SettingsManager.instance.OnMusicOptionChange += ChangeMusicOption;
        _audioSource.Play();
    }

    void OnDisable()
    {
        SettingsManager.instance.OnMusicVolumeChange -= ChangeMusicVolume;
        SettingsManager.instance.OnMusicOptionChange -= ChangeMusicOption;
    }

    private void ChangeMusicVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public void ChangeMusicOption(int index)
    {
        Debug.Log($"[BackgroundMusicManager] Changing music option to {index}");
        if (index != selectedIndex)
        {
            
            _audioSource.Stop();
            _audioSource.clip = _musicClips[index];
            _audioSource.Play();
            selectedIndex = index;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
