using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public int moneyValue = 1; // probably should not be in "health" script, but this needs to be reworked anyways...
    public bool isBoss = false;
    [SerializeField] private EndGoal _endGoal;

    public event Action<Health> OnDeath;

    //probably not the place to store this but whatever
    public GameObject damageParticleSystemPrefab;
    public ParticleSystem damageParticle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //currentHealth = maxHealth;
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            // death stuff later
            BasicRatNavigation navigation = GetComponent<BasicRatNavigation>();
            if (navigation)
            {
                navigation.BeginDeath();
            }
            MyLogger.Info($"{gameObject.name} dying!");
            PlayerMoney.Instance?.AddMoney(moneyValue);
            OnDeath?.Invoke(this);
            if (isBoss && _endGoal != null)
            {
                _endGoal.Unlock();
            }
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage, Vector3 impactPoint = default(Vector3) , Vector3 impactNormal = default(Vector3))
    {
        MyLogger.Info($"Damage dealt to {gameObject.name}: {damage}");
        currentHealth -= damage;

        if (impactPoint != default(Vector3) && impactNormal != default(Vector3))
        {
            if (!damageParticle)
            {
                damageParticle = Instantiate(damageParticleSystemPrefab).GetComponent<ParticleSystem>();
                damageParticle.transform.SetParent(transform, true);
            }
            //Debug.Log("Got normal and stuff");
            damageParticle.transform.position = impactPoint;
            damageParticle.transform.forward = impactNormal;
            damageParticle.Play();
        }
    }
}
