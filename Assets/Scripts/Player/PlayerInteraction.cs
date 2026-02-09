using UnityEngine;
public class PlayerInteraction : MonoBehaviour
{
    private Interactable _currentInteractable;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Object {other.gameObject.name} entered interaction box");
        if(other.TryGetComponent(out Interactable interactable))
        {
            Debug.Log("Interactable enters range");
            _currentInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable) &&
            interactable == _currentInteractable)
        {
            Debug.Log("Interactable exits range");
            _currentInteractable = null;
        }
    }

    void OnEnable()
    {
        InputHandler.Instance?.OnInteractInput += HandleInteract;
    }

    void OnDisable()
    {
        InputHandler.Instance?.OnInteractInput -= HandleInteract;
    }

    private void HandleInteract()
    {
        Debug.Log($"PlayerInteractable in HandleInteract, _currentInteractable? {_currentInteractable == null}");
        _currentInteractable?.Interact();
    }
}
