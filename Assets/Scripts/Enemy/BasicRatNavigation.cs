using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BasicRatNavigation : MonoBehaviour
{
    public static int instanceCount = 0;
    
    public Transform target;
    public NavMeshAgent agent;
    public bool canMove = true;
    public bool isAttacking = false;
    public float destinationCalcInterval = 0.25f;
    private float _timeOfLastDestinationCalc;

    [SerializeField] private BasicRatAttack _ratCombat;
    [SerializeField] private TMP_Text _debugText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _ratCombat = GetComponent<BasicRatAttack>();
        target = PlayerHealth.GetPlayerHealthInstance().transform;
        instanceCount++;
        MyLogger.Info($"Current rat count: {instanceCount}");
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += ClearNavTarget;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= ClearNavTarget;
    }

    private void ClearNavTarget(PlayerHealth playerHealth)
    {
        canMove = false;
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_ratCombat != null)
            canMove = !_ratCombat.isAttacking;
        Vector3 targetLocation;
        if (target && target.gameObject.activeInHierarchy)
            targetLocation = target.position;
        else
            targetLocation = new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f));
        if (canMove && (Time.time - _timeOfLastDestinationCalc) > destinationCalcInterval)
        {
            agent.SetDestination(targetLocation);
            _timeOfLastDestinationCalc = Time.time;
        }
    }

    public void BeginDeath()
    {
        instanceCount--;
        MyLogger.Info($"Current rat count: {instanceCount}");
        if (instanceCount < 0)
        {
            MyLogger.Error("instanceCount is less than 0");
            instanceCount = 0;
        }
    }
}
