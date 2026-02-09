using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    public event Action OnInteract;

    public void Interact()
    {
        OnInteract?.Invoke();
    }
}
