using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] spawnTransforms;
    public string name = "DefaultRoom";
    public bool finalRoomEvent = false;
    [SerializeField] private GameObject _bossRat;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player entered room {name} / {other.gameObject.name}");
        if(SpawnerSystem.Instance != null )
        {
            SpawnerSystem.Instance.SetCurrentRoom(this);
        }

        if (finalRoomEvent && _bossRat != null)
        {
            _bossRat.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Player exit room {name}");
        // clear current room? or is it enough that the next room will set current room
    }
}
