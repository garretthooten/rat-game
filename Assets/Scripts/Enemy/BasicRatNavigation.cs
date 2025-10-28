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

    // Update is called once per frame
    void Update()
    {
        if (_ratCombat != null)
            canMove = !_ratCombat.isAttacking;
        if (target && canMove)
        {
            agent.SetDestination(target.position);
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
