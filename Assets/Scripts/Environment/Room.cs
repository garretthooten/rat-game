using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] spawnTransforms;
    public string name = "DefaultRoom";

    //[SerializeField] private string _playerTag; I dont think I need this I can just manage collision layers
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player entered room {name}");
        if(SpawnerSystem.Instance != null )
        {
            SpawnerSystem.Instance.SetCurrentRoom(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Player exit room {name}");
        // clear current room? or is it enough that the next room will set current room
    }
}
