using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    public RectTransform healthbarForeground;

    public static HealthbarUI Instance { get; private set; }

    private float _maxWidth = 400f;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        healthbarForeground.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _maxWidth);
    }

    private void UpdateHealthbar(HealthChange change)
    {
        float width = (change.updatedHealth / PlayerHealth.instance.maxHealth) * _maxWidth;
        healthbarForeground.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PlayerHealth.instance != null)
        {
            PlayerHealth.OnPlayerHealthChange += UpdateHealthbar;
        }
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerHealthChange -= UpdateHealthbar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
