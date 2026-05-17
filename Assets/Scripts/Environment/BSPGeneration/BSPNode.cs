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
    void Awake()
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

    // Returns all leaf nodes adjacent to this leaf node.
    // Should only be called on leaf nodes (left == null && right == null).
    public List<BSPNode> GetAdjacencies()
    {
        var adjacent = new List<BSPNode>();
        Bounds myBounds = new Bounds(transform.position, transform.localScale);

        BSPNode current = this;
        while (current.parent != null)
        {
            BSPNode par = current.parent;
            BSPNode siblingSubtree = (par.left == current) ? par.right : par.left;

            foreach (var leaf in GetLeaves(siblingSubtree))
            {
                Bounds leafBounds = new Bounds(leaf.transform.position, leaf.transform.localScale);
                if (SharesEdge(myBounds, leafBounds))
                    adjacent.Add(leaf);
            }

            current = par;
        }

        return adjacent;
    }

    private List<BSPNode> GetLeaves(BSPNode node)
    {
        var leaves = new List<BSPNode>();
        if (node == null) return leaves;

        if (node.left == null && node.right == null)
            leaves.Add(node);
        else
        {
            leaves.AddRange(GetLeaves(node.left));
            leaves.AddRange(GetLeaves(node.right));
        }
        return leaves;
    }

    // Two leaf bounds share an edge if they touch on one axis (within the padding gap)
    // and their ranges overlap on the other axis.
    private bool SharesEdge(Bounds a, Bounds b)
    {
        // Each leaf is shrunk by leafPadding on x and z, so adjacent rooms have a
        // gap of exactly leafPadding between their surfaces. The tolerance absorbs that gap.
        float tolerance = leafPadding + 0.001f;

        bool touchX = Mathf.Abs(a.max.x - b.min.x) <= tolerance || Mathf.Abs(b.max.x - a.min.x) <= tolerance;
        bool overlapZ = a.min.z < b.max.z && b.min.z < a.max.z;

        bool touchZ = Mathf.Abs(a.max.z - b.min.z) <= tolerance || Mathf.Abs(b.max.z - a.min.z) <= tolerance;
        bool overlapX = a.min.x < b.max.x && b.min.x < a.max.x;

        return (touchX && overlapZ) || (touchZ && overlapX);
    }
}
