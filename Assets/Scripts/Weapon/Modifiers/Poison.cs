using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class Poison : MonoBehaviour
{
    private Gun _gun;
    [SerializeField] private float _damagePerTick = 1f;
    [SerializeField] private float _tickInterval = .2f;
    [SerializeField] private float _duration = 10.0f;
    private void Start()
    {
        _gun = GetComponent<Gun>();
        if (_gun != null)
            _gun = GetComponent<Gun>();

        _gun.OnDamageDealt += ApplyEffect;
    }

    private void OnEnable()
    {
        if( _gun != null )
            _gun = GetComponent<Gun>();

        _gun.OnDamageDealt += ApplyEffect;
    }

    private void OnDisable()
    {
        _gun.OnDamageDealt -= ApplyEffect;
    }

    void ApplyEffect(DamageInstance damageInstance)
    {
        Debug.Log("Applying poison effect");
        StartCoroutine(PoisonEffect(_duration, damageInstance.EnemyHealth));
    }

    IEnumerator PoisonEffect(float duration, Health enemyHealth)
    {
        float startTime = Time.time;
        while(Time.time - startTime <= _duration)
        {
            enemyHealth.TakeDamage(_damagePerTick);
            DamageIndicator indicator = DamageIndicatorPool.Instance.Get();
            if(indicator != null )
            {
                indicator.Display(Color.green, _damagePerTick, enemyHealth.transform.position);
            }
            yield return new WaitForSeconds(_tickInterval);
        }
    }
}
