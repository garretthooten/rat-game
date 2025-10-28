using UnityEngine;
using System.Collections;

public class BasicRatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ratPrefab;
    [SerializeField] private int _ratsPerSecond = 1;
    [SerializeField] private Transform _ratSpawnPoint;
    
    private float _timeBetweenSpawns;
    private float _timeOfLastSpawn;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _timeBetweenSpawns = 1 / _ratsPerSecond;
        MyLogger.Info($"Creating rat spawner with {_ratsPerSecond} RPS, one spawn every {_timeBetweenSpawns} seconds");
        StartCoroutine(SpawnRoutine());
    }

    void OnDestroy()
    {
        StopCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Instantiate(_ratPrefab, _ratSpawnPoint.position, _ratSpawnPoint.rotation);
            yield return new WaitForSeconds(_timeBetweenSpawns);
        }
    }
}
