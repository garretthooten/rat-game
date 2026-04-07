using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicRatAttack : MonoBehaviour
{
    
    [SerializeField] private PlayerHealth _playerHealth;
    private BasicRatNavigation _basicRatNavigation;
    [SerializeField] private Transform _playerTransform;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    [SerializeField] private SingleHitBox _attackBox;
    //[SerializeField] private BoxCollider _attackBox;
    [SerializeField] private float attackDistance = 2.0f;
    [SerializeField] private float _attackDuration = 1.0f;
    [SerializeField] private float _attackCooldown = 2.0f;
    [FormerlySerializedAs("_isAttacking")] public bool isAttacking = false;
    private float _timeOfLastAttack;

    public void Awake()
    {
        if (!_attackBox)
        {
            MyLogger.Error("_attackBox is missing!");
        }

        if (!_audioSource)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        if (!_basicRatNavigation)
        {
            _basicRatNavigation = GetComponent<BasicRatNavigation>();
        }
    }

    public void OnEnable()
    {
        _attackBox.DisableAttack();
        isAttacking = false;
    }

    public void OnDisable()
    {
        _attackBox.DisableAttack();
        isAttacking = false;
    }
    
    public void Start()
    {
        _playerHealth = PlayerHealth.GetPlayerHealthInstance();
        _playerTransform = _playerHealth.transform;
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
        //Debug.Log($"distanceToPlayer: {distanceToPlayer}\nattackDistance: {attackDistance}\nisAttacking: {isAttacking}\nPlayerHealth.IsDead: {PlayerHealth.IsDead}");
        if (distanceToPlayer <= attackDistance && !isAttacking && !PlayerHealth.IsDead)
        {
            float timeSinceLastAttack = Time.time - _timeOfLastAttack;
            MyLogger.Info($"Within attack distance, time since attack: {timeSinceLastAttack}s");
            if (timeSinceLastAttack < _attackCooldown)
                return; // not going to work when there is more logic for more complex behaviors, but this is good for now
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        _timeOfLastAttack = Time.time;
        _audioSource.PlayOneShot(_audioClip, SettingsManager.instance.sfxVolume);
        _attackBox.EnableAttack();
        yield return new WaitForSeconds(_attackDuration);
        _attackBox.DisableAttack();
        isAttacking = false;
    }
    
}
