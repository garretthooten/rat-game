using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DamageIndicatorPool : MonoBehaviour
{
    public static DamageIndicatorPool Instance { get; private set; }
    
    public GameObject DamageIndicatorPrefab;
    public int instanceCount = 20;

    public List<DamageIndicator> _indicators;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _indicators = new List<DamageIndicator>();
        for(int i = 0; i < instanceCount; i++)
        {
            GameObject instance = Instantiate(DamageIndicatorPrefab);
            DamageIndicator indicator = instance.GetComponent<DamageIndicator>();

            if (!indicator)
                Debug.LogError("No Indicator component!");

            _indicators.Add(indicator);
            
            //_indicators.Append(instance.GetComponent<DamageIndicator>());
        }
    }

    public DamageIndicator Get()
    {
        foreach (var indicator in _indicators)
        {
            if(!indicator.isEnabled)
            {
                return indicator;
            }
        }
        Debug.LogError("No suitable DamageIndicator for request");
        return null;
    }
}
