using UnityEngine;
using System.Collections;

public class BasicRatSpawner : MonoBehaviour
{
    public int currentRatCount = 0;
    public int maxRatCount = 100;

    [SerializeField] private ObjectPool _ratPool;
    [SerializeField] private GameObject _ratPrefab;
    [SerializeField] private int _ratsPerSecond = 1;
    [SerializeField] private Transform _ratSpawnPoint;
    
    private float _timeBetweenSpawns;
    private float _timeOfLastSpawn;
    private Coroutine _spawnCoroutine;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _timeBetweenSpawns = 1f / _ratsPerSecond;
        MyLogger.Info($"Creating rat spawner with {_ratsPerSecond} RPS, one spawn every {_timeBetweenSpawns} seconds");
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    void OnDisable()
    {
        if(_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentRatCount < maxRatCount)
            {
                GameObject rat = _ratPool.GetObject();
                if (rat != null)
                {
                    rat.transform.position = _ratSpawnPoint.position;
                    rat.transform.rotation = _ratSpawnPoint.rotation;
                    //GameObject rat = Instantiate(_ratPrefab, _ratSpawnPoint.position, _ratSpawnPoint.rotation);
                    rat.GetComponent<Health>().OnDeath += HandleRatDeath;
                    currentRatCount++;
                }
            }
            yield return new WaitForSeconds(_timeBetweenSpawns);
        }
    }

    void HandleRatDeath(Health health)
    {
        health.OnDeath -= HandleRatDeath;
        currentRatCount--;
    }
}
