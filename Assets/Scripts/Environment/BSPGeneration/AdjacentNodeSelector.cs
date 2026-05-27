using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AdjacentNodeSelector : MonoBehaviour
{
    private InputHandler _inputHandler;

    [SerializeField] private Material _selectedMaterial, _defaultMaterial;
    
    List<BSPNode> selectedNodes = new List<BSPNode>();
    
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
        if (selectedNodes.Count > 0)
        {
            ClearSelections();
        }
        
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        RaycastHit hit;
        var originMouse = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(originMouse, out hit, 999f))
        {
            Debug.Log(hit.transform.name);
            _defaultMaterial =  hit.transform.gameObject.GetComponent<Renderer>().material;
            // nasty but necessary for now
            var node = hit.transform.gameObject.GetComponent<BSPNode>();
            if (node == null)
            {
                Debug.LogError($"No BSPNode found on {hit.transform.name}");
                return;
            }

            var adjacencies = node.GetAdjacencies();
            Debug.Log($"Have {adjacencies.Count} adjacencies");
            hit.transform.gameObject.GetComponent<Renderer>().material = _selectedMaterial;
            selectedNodes.Add(node);
            foreach (var adjacency in adjacencies)
            {
                adjacency.transform.gameObject.GetComponent<Renderer>().material = _selectedMaterial;
                selectedNodes.Add(adjacency);
            }
        }
    }

    void ClearSelections()
    {
        foreach (var node in selectedNodes)
        {
            node.transform.gameObject.GetComponent<Renderer>().material = _defaultMaterial;
        }
        selectedNodes.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
