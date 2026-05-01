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
        Debug.Log("splitpercent: " + splitPercent);

        Vector3 leftOrigin, leftPosition, rightOrigin, rightPosition, leftScale, rightScale;
        
        if (horizontal)
        {
            leftOrigin = transform.position - new Vector3(transform.localScale.x / 2f, transform.position.z, transform.position.z);
            leftScale = new Vector3(transform.localScale.x * splitPercent, transform.localScale.y,
                transform.localScale.z);
            leftPosition = new Vector3(leftOrigin.x + leftScale.x / 2, leftOrigin.y, leftOrigin.z);

            rightOrigin = new Vector3(leftPosition.x + (leftScale.x / 2), leftOrigin.y, leftOrigin.z);
            rightScale = new Vector3(transform.localScale.x * (1 - splitPercent),  transform.localScale.y, transform.localScale.z);
            rightPosition = new Vector3(rightOrigin.x + rightScale.x / 2, rightOrigin.y, rightOrigin.z);

            // var marker1 = MyLogger.MakeDebugSphere(leftOrigin, "leftOrigin");
            // var marker2 = MyLogger.MakeDebugSphere(leftPosition, "leftPosition");
            // var marker3 = MyLogger.MakeDebugSphere(rightOrigin, "rightOrigin");
            // var marker4 = MyLogger.MakeDebugSphere(rightPosition, "rightPosition");
            
            var left = CreateChildNode(leftPosition, leftScale, this.myDepth + 1, totalTreeDepth);
            var right = CreateChildNode(rightPosition, rightScale, this.myDepth + 1, totalTreeDepth);

            left = left;
            right = right;
            
            
        }
        else
        {
            leftOrigin = transform.position - new Vector3(transform.position.x, transform.position.y, transform.localScale.z / 2f);
            leftScale = new Vector3(transform.localScale.x, transform.localScale.y,
                transform.localScale.z * splitPercent);
            leftPosition = new Vector3(leftOrigin.x, leftOrigin.y, leftOrigin.z + leftScale.z / 2);

            rightOrigin = new Vector3(leftOrigin.x, leftOrigin.y, leftPosition.z + (leftScale.z / 2));
            rightScale = new Vector3(transform.localScale.x,  transform.localScale.y, transform.localScale.z * (1 - splitPercent));
            rightPosition = new Vector3(rightOrigin.x, rightOrigin.y, rightOrigin.z + rightScale.z / 2);

            // var marker1 = MyLogger.MakeDebugSphere(leftOrigin, "leftOrigin");
            // var marker2 = MyLogger.MakeDebugSphere(leftPosition, "leftPosition");
            // var marker3 = MyLogger.MakeDebugSphere(rightOrigin, "rightOrigin");
            // var marker4 = MyLogger.MakeDebugSphere(rightPosition, "rightPosition");
            
            var left = CreateChildNode(leftPosition, leftScale, this.myDepth + 1, totalTreeDepth);
            var right = CreateChildNode(rightPosition, rightScale, this.myDepth + 1, totalTreeDepth);

            left = left;
            right = right;
        }
        GetComponent<MeshRenderer>().enabled = false;
    }

    BSPNode CreateChildNode(Vector3 position, Vector3 size, int depth, int totalDepth)
    {
        GameObject childNode = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //childNode.transform.SetParent(transform, false);
        childNode.transform.position = position;
        childNode.transform.localScale = size;
        var node = childNode.AddComponent(typeof(BSPNode)) as BSPNode;
        if (node != null)
        {
            node.Init(depth, totalDepth, this);
        }
        return node;
    }
}
