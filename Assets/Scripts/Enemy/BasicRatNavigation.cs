using UnityEngine;
using UnityEngine.AI;

public class BasicRatNavigation : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    public bool canMove = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
         if (target && canMove)
         {
             agent.SetDestination(target.position);
         }
    }
}
