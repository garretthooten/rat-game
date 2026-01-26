using System;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public static event Action<PlayerHealth> OnPlayerDeath;
    public static bool IsDead = false;

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
            Die();
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

    private void Die()
    {
        Debug.Log("Player Dying!");
        IsDead = true;
        OnPlayerDeath?.Invoke(this);
        gameObject.SetActive(false);
    }
}
