using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    public float damage = 10f;

    void OnTriggerEnter(Collider other)
    {
        // assuming its on the rat layer and has a health component:
        if (other != null)
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
