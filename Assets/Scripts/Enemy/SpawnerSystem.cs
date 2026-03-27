using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerSystem : MonoBehaviour
{
    public static SpawnerSystem Instance { get; private set; }

    //public GameObject ratPrefab;
    public ObjectPool ratPool;

    public Room currentRoom;
    public int wavesComplete = 0;
    public int startingWaveMaxRats = 100;
    public int startingWaveSpawnRate = 10; // rate in seconds
    public bool waveInProgress = false;

    [SerializeField] private float _waveDuration = 60f;
    [SerializeField] private float _timeBetweenWaves = 30f;
    [SerializeField] private bool _startWaveOnInit = true;

    [SerializeField] private Text _debugText;

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
        if(_startWaveOnInit)
        {
            StartWaveRoutine();
        }
    }

    public void StartWaveRoutine()
    {
        if(_waveRoutine == null)
        {
            if(ratPool != null)
            {
                _waveRoutine = StartCoroutine(WaveRoutine());
                return;
            }
            Debug.LogError("No rat pool assigned, aborting spawn routine start");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRatAtRandom()
    {
        int chosenTransformIndex = Random.Range(0, currentRoom.spawnTransforms.Length);
        Debug.Log($"Chosen index: {chosenTransformIndex} (length {currentRoom.spawnTransforms.Length})");
        GameObject rat = ratPool.GetObject();
        rat.transform.position = currentRoom.spawnTransforms[chosenTransformIndex].position;
    }

    IEnumerator WaveRoutine()
    {
        Debug.Log("Starting to spawn routine");
        yield return null;
        
    }
}
