using UnityEngine;

public class BSPNode : MonoBehaviour
{
    public bool isRoot = false;
    public int myDepth, totalTreeDepth;
    public BSPNode parent, left, right;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isRoot)
        {
            Debug.Log($"transform.localScale: {transform.localScale}\ntransform.lossyScale: {transform.lossyScale}");
            //Split(1, 2);
            myDepth = 0;
            parent = null;
            Init(myDepth, totalTreeDepth);
        }
    }

    public void Init(int depth, int totalDepth, BSPNode p = null)
    {
        myDepth = depth;
        totalTreeDepth = totalDepth;
        parent = p;
        if (myDepth < totalTreeDepth)
        {
            Split();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Split()
    {
        GetComponent<MeshRenderer>().enabled = false;
        
        bool horizontal = Random.Range(0f, 9f) > 4.5f; // true for horizontal, false for vertical
        float splitPercent = Random.Range(0.3f, 0.6f);

        Vector3 adjustedScale;
        Vector3 adjustedScale1;
        Vector3 adjustedPosition, adjustedPosition1, adjustedOrigin, adjustedOrigin1;
        
        if (true)
        {
            var parentWidth = transform.localScale.x;
            float minX = -parentWidth / 2f;
            float maxX = parentWidth / 2f;

            float splitX = Mathf.Lerp(minX, maxX, splitPercent);

// left
            float leftWidth = splitX - minX;
            float leftCenter = minX + leftWidth / 2f;

// right
            float rightWidth = maxX - splitX;
            float rightCenter = splitX + rightWidth / 2f;

            adjustedPosition = new Vector3(leftCenter, 0f, 0f);
            adjustedPosition1 = new Vector3(rightCenter, 0f, 0f);

            adjustedScale = new Vector3(leftWidth, 1f, 1f);
            adjustedScale1 = new Vector3(rightWidth, 1f, 1f);
        }
        else
        {
            adjustedOrigin = new Vector3(0f, 0f, -transform.localScale.z / 2);
            
            adjustedScale = new Vector3(1f, 1f, splitPercent);
            adjustedScale1 = new Vector3(1f, 1f, 1f - splitPercent);

            
            adjustedPosition = new Vector3(0f, 0f, adjustedOrigin.z + adjustedScale.z/2f);
            adjustedOrigin1 = new Vector3(0f, 0f, adjustedPosition.z + adjustedScale.z / 2);
            adjustedPosition1 = new Vector3(0f, 0f, adjustedOrigin1.z  + adjustedScale1.z / 2);
        }
        
        

        left = CreateChildNode(adjustedScale, adjustedPosition, myDepth + 1, totalTreeDepth);
        right = CreateChildNode(adjustedScale1, adjustedPosition1, myDepth + 1, totalTreeDepth);
    }

    BSPNode CreateChildNode(Vector3 size, Vector3 position, int depth, int totalDepth)
    {
        GameObject childNode = GameObject.CreatePrimitive(PrimitiveType.Cube);
        childNode.transform.SetParent(transform, false);
        childNode.transform.localPosition = position;
        childNode.transform.localScale = size;
        var node = childNode.AddComponent(typeof(BSPNode)) as BSPNode;
        if (node != null)
        {
            node.Init(depth, totalDepth, this);
        }
        return node;
    }
}
