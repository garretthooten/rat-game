using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TestAgentNavigation : MonoBehaviour
{
    public Transform destinationTransform;
    public float navRecalcWaitTime = 0.25f;

    private NavMeshAgent _agent;
    private IEnumerator _coroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(NavigationRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator NavigationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(navRecalcWaitTime);
            _agent.SetDestination(destinationTransform.position);
        }
    }
}
