using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public float maxHealth = 100f;
    public float currentHealth;

    public TMP_Text debugText;

    public static PlayerHealth GetPlayerHealthInstance()
    {
        return instance;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // death stuff
            MyLogger.Info("Player dying!");
            Destroy(this.gameObject);
        }

        if (debugText)
        {
            string healthText = $"Health: {currentHealth}/{maxHealth}";
            debugText.text = healthText;
        }
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
    }
}
