using UnityEngine;
public class PlayerInteraction : MonoBehaviour
{
    private Interactable _currentInteractable;

    void Start()
    {
        if(InputHandler.Instance != null)
        {
            InputHandler.Instance.OnInteractInput += HandleInteract;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Object {other.gameObject.name} entered interaction box");
        if(other.TryGetComponent(out Interactable interactable))
        {
            Debug.Log($"Interactable enters range: {interactable.gameObject.name}");
            _currentInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable) &&
            interactable == _currentInteractable)
        {
            Debug.Log($"Interactable exits range: {interactable.gameObject.name}");
            _currentInteractable = null;
        }
    }

    void OnEnable()
    {
        if(InputHandler.Instance != null)
        {
            InputHandler.Instance.OnInteractInput += HandleInteract;
        }
    }

    void OnDisable()
    {
        if(InputHandler.Instance != null)
            InputHandler.Instance.OnInteractInput -= HandleInteract;
    }

    private void HandleInteract()
    {
        _currentInteractable?.Interact();
    }
}
