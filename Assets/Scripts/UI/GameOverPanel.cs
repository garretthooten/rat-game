using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += UpdateWaveInfo;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= UpdateWaveInfo;
    }

    void UpdateWaveInfo(PlayerHealth health)
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
