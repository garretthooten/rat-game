using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            // death stuff later
            MyLogger.Info($"{gameObject.name} dying!");
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        MyLogger.Info($"Damage dealt to {gameObject.name}: {damage}");
        currentHealth -= damage;
    }
}
