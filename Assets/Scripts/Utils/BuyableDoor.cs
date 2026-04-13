using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Interactable))]
public class BuyableDoor : MonoBehaviour
{
    public int price = 20;

    private Interactable _interactable;

    [SerializeField] private Text _text;

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    private void OnEnable()
    {
        _interactable.OnInteract += Purchase;
        if (_text != null)
            _text.text = $"${price}";
    }

    private void OnDisable()
    {
        _interactable.OnInteract -= Purchase;
    }

    private void Purchase()
    {
        Debug.Log("Purchasing Door");
        if(PlayerMoney.Instance)
        {
            if (PlayerMoney.Instance.currentMoney >= price)
            {
                //PlayerMoney.Instance.currentMoney -= price;
                PlayerMoney.Instance.RemoveMoney(price);
                gameObject.SetActive(false);
            }
            else 
            {
                Debug.Log("Purchase failed, player does not have enough money");
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
