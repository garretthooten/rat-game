using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }
    public int currentMoney = 0;
    public event Action OnMoneyChange;

    [SerializeField] private TMP_Text _debugText;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_debugText != null)
            OnMoneyChange += UpdateMoneyDebugText;
    }

    private void OnDisable()
    {
        if (_debugText != null)
            OnMoneyChange -= UpdateMoneyDebugText;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateMoneyDebugText()
    {
        _debugText.text = $"Money: {currentMoney}";
    }

    //public void GetCurrentBalance()

    public void AddMoney(int value)
    {
        currentMoney += value;
        OnMoneyChange?.Invoke();
    }

    public void RemoveMoney(int value)
    {
        currentMoney -= value;
        if (currentMoney < 0)
            currentMoney = 0;
        OnMoneyChange?.Invoke();
    }
}
