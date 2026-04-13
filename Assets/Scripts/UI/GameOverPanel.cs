using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private bool _isVictoryScreen;
    [SerializeField] private EndGoal _endGoal;
    
    void OnEnable()
    {
        
        if (_isVictoryScreen)
        {
            _endGoal.OnVictory += UpdateWaveInfoVictory;
        }
        else
            PlayerHealth.OnPlayerDeath += UpdateWaveInfo;
    }

    void OnDisable()
    {
        
        if (_isVictoryScreen)
        {
            _endGoal.OnVictory -= UpdateWaveInfoVictory;
        }
        else
        {
            PlayerHealth.OnPlayerDeath -= UpdateWaveInfo;
        }
    }

    void UpdateWaveInfo(PlayerHealth health)
    {
        // enable panel
        transform.GetChild(0).gameObject.SetActive(true);
        _text.text = $"Waves Complete: {SpawnerSystem.Instance.wavesComplete}";
    }
    
    void UpdateWaveInfoVictory()
    {
        // enable panel
        transform.GetChild(0).gameObject.SetActive(true);
        _text.text = $"Waves Complete: {SpawnerSystem.Instance.wavesComplete}";
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
