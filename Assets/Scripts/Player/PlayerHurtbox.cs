using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    public float damage = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Player Hurtbox Collided with {other.gameObject.name}");
        // assuming its on the rat layer and has a health component:
        if (other != null)
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
