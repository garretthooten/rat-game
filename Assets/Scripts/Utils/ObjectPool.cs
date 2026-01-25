using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int poolCount = 10;
    public bool ready = false;

    private List<GameObject> _pooledObjects;

    private void Awake()
    {
        _pooledObjects = new List<GameObject>();
        for(int i = 0; i < poolCount; i++)
        {
            GameObject instance = Instantiate(objectToPool);
            instance.SetActive(false);
            _pooledObjects.Add(instance);
        }
        ready = true;
    }

    public GameObject GetObject()
    {
        if (ready)
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (_pooledObjects[i] != null && !_pooledObjects[i].activeInHierarchy)
                {
                    _pooledObjects[i].SetActive(true);
                    return _pooledObjects[i];
                }
            }
            Debug.LogError("No suitable gameobject found in pool");
            return null;
        }
        Debug.LogError("ObjectPool not ready before GetObject invoked");
        return null;
    }
}
