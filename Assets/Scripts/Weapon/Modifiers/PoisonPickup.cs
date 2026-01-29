using UnityEngine;

public class PoisonPickup : MonoBehaviour
{
    public string playerTag = "Player";

    public bool shedEffect = false;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(playerTag))
        {
            Debug.Log("Player walked over poison effect");
            if (!shedEffect)
            {
                if (!other.GetComponent<PlayerCombat>().currentGun.gameObject.GetComponent<Poison>())
                    other.GetComponent<PlayerCombat>().currentGun.gameObject.AddComponent<Poison>();
                else
                {
                    Debug.Log("Current gun already had poison");
                }
            }
            else
            {
                Destroy(other.GetComponent<PlayerCombat>().currentGun.gameObject.GetComponent<Poison>());
            }
        }
    }
}
