using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    private  InputHandler _input;
    private bool _lastAttack;
    private GameObject _cursorMarker;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private int _ignoreLayer = 2;
    [SerializeField] private TMP_Text _debugText;
    public bool isAttacking;
    public Gun currentGun;
    public Gun[] weaponInventory;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _input = GetComponent<InputHandler>();
    }

    void MakeCursorMarker(Vector3 point)
    {
        if (!_cursorMarker)
        {
            _cursorMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _cursorMarker.layer = _ignoreLayer;
        }

        _cursorMarker.transform.position = point;
    }

    void DeleteCursorMarker()
    {
        if (_cursorMarker)
        {
            Destroy(_cursorMarker);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_input.attack)
        {
            isAttacking = true;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            RaycastHit hit;
            var originMouse = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(originMouse, out hit))
            {
                // visualize hit point
                MakeCursorMarker(hit.point);
                
                Vector3 playerToHitDirection = hit.point - transform.position;
                Vector3 normalizedDirection = playerToHitDirection.normalized;
                gameObject.transform.forward = new Vector3(normalizedDirection.x, 0, normalizedDirection.z);
                
                if (currentGun)
                {
                    //MyLogger.Info("Pulling gun trigger");
                    //MyLogger.Info($"Pulling gun trigger");
                    //currentGun.Fire(hit.point);
                    currentGun.PullTrigger(hit.point);
                }
            }
            
        }
        else
        {
            isAttacking = false;
            if (_lastAttack && currentGun)
            {
                DeleteCursorMarker();
                currentGun.ReleaseTrigger();
            }
        }
        _lastAttack = _input.attack;

        if (_input.reload && currentGun)
        {
            currentGun.Reload();
        }

        if (_debugText)
        {
            _debugText.text = MakeDebugString();
        }
    }

    private string MakeDebugString()
    {
        string result = $"isAttacking: {isAttacking}\n";
        if (!currentGun)
        {
            result = result + "No weapon equipped\n";
        }
        else
        {
            result = result + currentGun.MakeDebugString();
        }

        return result;
    }
}
