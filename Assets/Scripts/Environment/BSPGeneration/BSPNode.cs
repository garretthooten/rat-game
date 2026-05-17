using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BSPNode : MonoBehaviour
{
    public static List<BSPNode> GeneratedNodes;
    
    public bool isRoot = false;
    public int myDepth, totalTreeDepth;
    public float leafPadding = 0.01f;
    public BSPNode parent, left, right;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isRoot)
        {
            Debug.Log($"transform.localScale: {transform.localScale}\ntransform.lossyScale: {transform.lossyScale}");
            myDepth = 0;
            parent = null;
            Init(myDepth, totalTreeDepth, leafPadding);
        }
    }

    public void Init(int depth, int totalDepth, float padding, BSPNode p = null)
    {
        myDepth = depth;
        totalTreeDepth = totalDepth;
        parent = p;
        leafPadding = padding;
        if (myDepth < totalTreeDepth)
        {
            Split();
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x - leafPadding,  transform.localScale.y, transform.localScale.z - leafPadding);
            //GeneratedNodes.Append(this);
            GeneratedRoom.GenerateRoom(gameObject);
        }
    }

    public void DisableSelf()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Split()
    {
        GetComponent<MeshRenderer>().enabled = false;

        bool horizontal = Random.value > 0.5f;
        float splitPercent = Random.Range(0.4f, 0.5f);
        Debug.Log("splitpercent: " + splitPercent);

        Bounds bounds = new Bounds(transform.position, transform.localScale);
        Bounds leftBounds, rightBounds;

        if (horizontal)
        {
            float leftWidth = bounds.size.x * splitPercent;
            float rightWidth = bounds.size.x - leftWidth;

            Vector3 leftSize = new Vector3(leftWidth, bounds.size.y, bounds.size.z);
            Vector3 leftCenter = bounds.min + new Vector3(leftWidth / 2f, bounds.size.y / 2f, bounds.size.z / 2f);
            leftBounds = new Bounds(leftCenter, leftSize);
            
            Vector3 rightSize = new Vector3(rightWidth, bounds.size.y, bounds.size.z);
            Vector3 rightCenter = bounds.min + new Vector3(leftWidth + rightWidth / 2f, bounds.size.y / 2f, bounds.size.z / 2f);
            rightBounds = new Bounds(rightCenter, rightSize);
        }
        else
        {
            float leftWidth = bounds.size.z * splitPercent;
            float rightWidth = bounds.size.z - leftWidth;

            Vector3 leftSize = new Vector3(bounds.size.x, bounds.size.y, leftWidth);
            Vector3 leftCenter = bounds.min + new Vector3(bounds.size.x / 2f, bounds.size.y / 2f, leftWidth / 2f);
            leftBounds = new Bounds(leftCenter, leftSize);
            
            Vector3 rightSize = new Vector3(bounds.size.x, bounds.size.y, rightWidth);
            Vector3 rightCenter = bounds.min + new Vector3(bounds.size.x / 2f, bounds.size.y / 2f, leftWidth + rightWidth / 2f);
            rightBounds = new Bounds(rightCenter, rightSize);
        }

        this.left = CreateChildNode(leftBounds, myDepth + 1, totalTreeDepth);
        this.right = CreateChildNode(rightBounds, myDepth + 1, totalTreeDepth);
    }

    BSPNode CreateChildNode(Bounds bounds, int depth, int totalDepth)
    {
        GameObject childNode = GameObject.CreatePrimitive(PrimitiveType.Cube);
        childNode.transform.position = bounds.center;
        childNode.transform.localScale = bounds.size;
        var node = childNode.AddComponent(typeof(BSPNode)) as BSPNode;
        if (node != null)
        {
            node.Init(depth, totalDepth, leafPadding, this);
        }
        return node;
    }
}
