using UnityEngine;

public class BasicCameraFollow : MonoBehaviour
{
    public Transform followTransform;
    public Vector3 positionOffset = new Vector3(0f, 1f, 0f);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followTransform.position + positionOffset;
    }
}
