using System.Globalization;
using UnityEngine;

public class EnemyItemDrops : MonoBehaviour
{
    public GameObject[] objectsToDrop;
    public float[] percentChanceToDrop;


    private void OnEnable()
    {
        if (objectsToDrop.Length != percentChanceToDrop.Length)
        {
            Debug.LogError("objectsToDrop list and percentChanceToDrop list are not same length");
            return;
        }

        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.OnDeath += TryDropItem;
        }

    }

    private void OnDisable()
    {
        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.OnDeath -= TryDropItem;
        }
    }

    void TryDropItem(Health health)
    {
        for(int i = 0; i < objectsToDrop.Length; i++)
        {
            float number = Random.Range(0f, 100f);
            if(number < percentChanceToDrop[i])
            {
                GameObject droppedItem = Instantiate(objectsToDrop[i]);
                droppedItem.transform.position = transform.position;
            }
        }

        return;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
