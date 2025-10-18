using UnityEngine;

public class BasicRatAttack : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private Transform _playerTransform;
    
    [SerializeField] private float _attackDistance = 0.2f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _timeBetweenAttacks = 1f;
    [SerializeField] private BoxCollider _hurtbox;
    private BasicRatNavigation _navigation;
    private float _timeOfLastAttack;
    
    void Awake()
    {
        if (_hurtbox == null)
        {
            MyLogger.Error($"Hurtbox is null");
        }

        _playerHealth = PlayerHealth.instance;
        _playerTransform = _playerHealth.transform;
        if(_playerHealth == null)
        {
            MyLogger.Error($"PlayerHealth is null");
        }

        _timeOfLastAttack = Time.time;
    }

    void Update()
    {
        // float distance = Vector3.Distance(transform.position, _playerTransform.position);
        // if (distance <= _attackDistance)
        // {
        //     MyLogger.Info("Attacking player!");
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        MyLogger.Info($"Trigger enter {other.gameObject.name}");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playerHealth.Damage(_attackDamage);
        }
    }
}
