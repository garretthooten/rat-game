using System.Collections;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.UI;


public class SpawnerSystem : MonoBehaviour
{
    public enum WaveState {Waiting, InProgress, InBetween, Finished}
    public static SpawnerSystem Instance { get; private set; }

    //public GameObject ratPrefab;
    public ObjectPool ratPool;

    public Room currentRoom;
    public int wavesComplete = 0;
    public int startingWaveMaxRats = 100;
    public int startingWaveSpawnRate = 10; // rate in seconds
    public int ratsSpawnedThisWave = 0;
    public int ratsKilledThisWave = 0;
    public bool waveInProgress = false;
    public WaveState waveState = WaveState.Waiting;

    [SerializeField] private float _waveDuration = 60f;
    [SerializeField] private int _waveRatCount;
    [SerializeField] private float _timeBetweenWaves = 30f;
    [SerializeField] private bool _startWaveOnInit = true;

    [SerializeField] private Text _debugText;

    private float _timeOfWaveStart;
    private float _timeOfWaveEnd;

    private Coroutine _waveRoutine;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void SetCurrentRoom(Room room)
    {
        currentRoom = room; // does this need to be a function?
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;
        
        if(_startWaveOnInit)
        {
            StartWaveRoutine();
        }
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
    }

    public void StartWaveRoutine()
    {
        if(_waveRoutine == null && ((waveState != WaveState.InBetween) || (waveState != WaveState.InProgress)))
        {
            if(ratPool != null)
            {
                _waveRatCount = startingWaveMaxRats;
                _waveRoutine = StartCoroutine(WaveRoutine());
                return;
            }
            Debug.LogError("No rat pool assigned, aborting spawn routine start");
        }
        else
        {
            Debug.LogError("Failed condition to start wave routine");
        }
    }

    void OnPlayerDeath(PlayerHealth health)
    {
        // stop system
        StopWaveRoutine();
    }

    public void StopWaveRoutine()
    {
        if(_waveRoutine != null)
        {
            StopCoroutine(_waveRoutine);
            _waveRoutine = null;
        }
        waveState = WaveState.Finished;
    }

    //public void IncrementRatKilled

    // Update is called once per frame
    void Update()
    {
        if(_debugText != null)
        {
            string t;
            switch(waveState)
            {
                case WaveState.Waiting:
                    t = $"Wave not active";
                    break;
                case WaveState.InBetween:
                    t = $"Wave beginning in {_timeBetweenWaves - (Time.time - _timeOfWaveEnd)}s";
                    break;
                case WaveState.InProgress:
                    t = $"Wave in progress\nwave time: {Time.time-_timeOfWaveStart}\nrat spawned count: {ratsSpawnedThisWave}/{_waveRatCount}\n";
                    break;
                case WaveState.Finished:
                    t = $"Spawn waves finished";
                    break;
                default:
                    t = "No information";
                    break;
            }
            _debugText.text = t;
        }
    }

    void SpawnRatAtRandom()
    {
        if(currentRoom != null)
        {
            int chosenTransformIndex = Random.Range(0, currentRoom.spawnTransforms.Length);
            //Debug.Log($"Chosen index: {chosenTransformIndex} (length {currentRoom.spawnTransforms.Length})");
            GameObject rat = ratPool.GetObject();
            rat.transform.position = currentRoom.spawnTransforms[chosenTransformIndex].position;

            ratsSpawnedThisWave++;
        }
    }

    IEnumerator WaveRoutine()
    {
        Debug.Log($"Starting to spawn routine with {_waveRatCount} rats and duration {_waveDuration}");

        while(currentRoom == null)
        {
            yield return null;
        }
        while(true)
        {
            float spawnRate = _waveRatCount / _waveDuration;
            ratsSpawnedThisWave = 0;
            Debug.Log($"Starting wave {wavesComplete + 1} with rate {spawnRate}");
            _timeOfWaveStart = Time.time;
            waveState = WaveState.InProgress;
            while(Time.time < (_timeOfWaveStart +_waveDuration))
            {
                SpawnRatAtRandom();
                yield return new WaitForSeconds(1/spawnRate);
            }

            wavesComplete++;
            _timeOfWaveEnd = Time.time;
            waveState = WaveState.InBetween;
            Debug.Log($"Wave duration ended, waiting for {_timeBetweenWaves} before next wave");
            yield return new WaitForSeconds(_timeBetweenWaves);
        }
        
    }
}
