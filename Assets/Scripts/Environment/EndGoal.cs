using System;
using UnityEngine;

public class EndGoal : MonoBehaviour
{
    public event Action OnVictory;
    
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private bool _unlocked = false;
    [SerializeField] private Material _lockedMaterial;
    [SerializeField] private Material _unlockedMaterial;

    public void Unlock()
    {
        _unlocked = true;
        GetComponent<Renderer>().material = _unlockedMaterial;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered final goal trigger");
            if (_unlocked)
            {
                Victory();
            }
        }
    }

    public void Victory()
    {
        Debug.Log("Player achieved victory");
        _pauseManager.TogglePause(false);
        _victoryScreen.SetActive(true);
        OnVictory?.Invoke();
        
    }
}
