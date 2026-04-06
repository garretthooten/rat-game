using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool _isPaused = false;
    [SerializeField] private GameObject pausePanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputHandler.Instance.OnPauseInput += TogglePause;
    }

    void OnDisable()
    {
        InputHandler.Instance.OnPauseInput -= TogglePause;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
        pausePanel.SetActive(_isPaused);
    }
}
