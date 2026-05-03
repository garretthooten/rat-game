using UnityEngine;

public class GeneratedRoom : MonoBehaviour
{
    public float length, width;

    private GameObject _floor, _wall1, _wall2, _wall3, _wall4;
    private bool _isInitiated = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValidate()
    {
        BuildRoom();
    }

    public void Init()
    {
        _floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _isInitiated = true;
    }

    public void BuildRoom()
    {
        if (_isInitiated)
        {
            _floor.transform.localScale = new Vector3(length, 1f, width);
            //_wall1.transform.
        }
    }
}
