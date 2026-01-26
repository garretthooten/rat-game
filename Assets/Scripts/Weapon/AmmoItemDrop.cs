using UnityEngine;

public class AmmoItemDrop : MonoBehaviour
{

    public Gun.Firetype firetype;
    public int amount = 30;

    [SerializeField] private string _playerTag = "Player";

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            Debug.Log("Player walked over ammo pickup");
            other.GetComponent<PlayerCombat>()?.AddWeaponAmmo(firetype, amount);
            Destroy(gameObject);
        }
    }

}
