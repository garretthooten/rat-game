using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{
    public GameObject[] itemPool;
    public float[] itemDropPercents;

    private Health _health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _health = GetComponent<Health>();
        if(_health != null)
            _health.OnDeath += DropItem;
    }

    void OnDisable()
    {
        _health.OnDeath -= DropItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DropItem(Health health)
    {
        // drop item
        Debug.Log("Item drop");
    }
}
