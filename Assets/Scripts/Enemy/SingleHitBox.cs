using UnityEngine;

public class SingleHitBox : MonoBehaviour
{
    public float damage = 10f;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private bool _attackPerformed = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerHealth = PlayerHealth.GetPlayerHealthInstance();
    }

    public void EnableAttack()
    {
        MyLogger.Info("Enabling myself!");
        gameObject.SetActive(true);
        _attackPerformed = false;
    }

    public void DisableAttack()
    {
        MyLogger.Info("Disabling myself!");
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_attackPerformed)
        {
            MyLogger.Info($"Collision detected: {other.tag}, _attackPerformed: {_attackPerformed}");
            if (other.CompareTag("Player"))
            {
                _playerHealth.Damage(damage);
                _attackPerformed = true;
            }
        }
    }
}
