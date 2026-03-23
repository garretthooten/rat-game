using UnityEngine;
using UnityEngine.UI;

public class SpawnerSystem : MonoBehaviour
{
    public static SpawnerSystem Instance { get; private set; }

    public Room currentRoom;

    [SerializeField] private float _waveDuration = 60f;
    [SerializeField] private float _timeBetweenWaves = 30f;

    [SerializeField] private Text _debugText;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
