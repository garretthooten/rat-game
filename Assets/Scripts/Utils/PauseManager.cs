using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool _isPaused = false;
    [SerializeField] private GameObject pausePanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputHandler.Instance.OnPauseInput += InternalTogglePause;
    }

    void OnDisable()
    {
        InputHandler.Instance.OnPauseInput -= InternalTogglePause;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InternalTogglePause()
    {
        TogglePause(false);
    }

    public void TogglePause(bool displayPanel = true)
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
        if(displayPanel)
            pausePanel.SetActive(_isPaused);
    }
}
