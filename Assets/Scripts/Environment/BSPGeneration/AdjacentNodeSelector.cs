using UnityEngine;
using UnityEngine.InputSystem;

public class AdjacentNodeSelector : MonoBehaviour
{
    private InputHandler _inputHandler;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputHandler = InputHandler.Instance;
        if (_inputHandler == null)
        {
            Debug.LogError("No InputHandler in scene!");
            return;
        }

        _inputHandler.OnAttackInput += SelectNode;

    }

    void OnDisable()
    {
        _inputHandler.OnAttackInput -= SelectNode;
    }

    void SelectNode()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        RaycastHit hit;
        var originMouse = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(originMouse, out hit, 999f))
        {
            Debug.Log(hit.transform.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
