using UnityEngine;

public class GeneratedRoom : MonoBehaviour
{
    public static void GenerateRoom(GameObject roomNode)
    {
        Bounds bounds = roomNode.GetComponent<MeshRenderer>().bounds;
        //Debug.Log($"center: {bounds.center}, size: {bounds.size}");

        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.position = bounds.center;
        floor.transform.localScale = bounds.size;
        floor.gameObject.name = "Floor";

        // GameObject wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // wall1.transform.position = new Vector3(bounds.center.x, bounds.center.y,
        //     bounds.center.z + bounds.size.z / 2);
        // wall1.transform.localScale = new Vector3(bounds.size.x, 2f, 1f);
        // wall1.gameObject.name = "wall1";
        //
        // GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // wall2.transform.position = new Vector3(bounds.center.x, bounds.center.y,
        //     bounds.center.z - bounds.size.z / 2);
        // wall2.transform.localScale = new Vector3(bounds.size.x, 2f, 1f);
        // wall2.gameObject.name = "wall2";
        //
        // GameObject wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // wall3.transform.position = new Vector3(bounds.center.x + bounds.size.x / 2, bounds.center.y,
        //     bounds.center.z);
        // wall3.transform.localScale = new Vector3(1f, 2f, bounds.size.z);
        // wall3.gameObject.name = "wall3";
        //
        // GameObject wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // wall4.transform.position = new Vector3(bounds.center.x - bounds.size.x / 2, bounds.center.y,
        //     bounds.center.z);
        // wall4.transform.localScale = new Vector3(1f, 2f, bounds.size.z);
        // wall4.gameObject.name = "wall4";
        //
        GameObject container = new GameObject();
        container.transform.position = bounds.center;
        container.name = "Room";
        GameObject geometryContainer = new GameObject();
        geometryContainer.transform.SetParent(container.transform);
        geometryContainer.transform.localPosition = Vector3.zero;
        geometryContainer.name = "Geometry";
        
        floor.transform.SetParent(geometryContainer.transform, true);
        // wall1.transform.SetParent(geometryContainer.transform, true);
        // wall2.transform.SetParent(geometryContainer.transform, true);
        // wall3.transform.SetParent(geometryContainer.transform, true);
        // wall4.transform.SetParent(geometryContainer.transform, true);
        
        Destroy(roomNode);
    }
}
