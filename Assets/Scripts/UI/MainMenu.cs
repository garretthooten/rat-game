using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _levelString = "Level1";
    [SerializeField] private GameObject _creditsDialog;
    [SerializeField] private GameObject _notesDialog;
    [SerializeField] private GameObject _settingsDialog;

    void Awake()
    {
        if (PlayerPrefs.GetInt("Initialized") != 1)
        {
            // player prefs not initialized, must make starting values
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(_levelString);
    }

    public void OpenCreditsDialog()
    {
        _creditsDialog.SetActive(true);
    }

    public void OpenNotesDialog()
    {
        _notesDialog.SetActive(true);
    }

    public void OpenSettingsDialog()
    {
        _settingsDialog.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
