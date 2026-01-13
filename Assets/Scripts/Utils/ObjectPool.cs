using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int poolCount = 10;

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
    }

    public GameObject GetObject()
    {
        for(int i = 0; i < _pooledObjects.Count; i++)
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
}
