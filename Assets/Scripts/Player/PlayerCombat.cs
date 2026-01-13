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
    [SerializeField] private Transform _weaponAttachTransform;
    private int _selectedWeaponIndex = 1;
    public bool isAttacking;
    public Gun currentGun;
    public GameObject[] weaponInventory; // must have gun component
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _input = GetComponent<InputHandler>();
        if (!_weaponAttachTransform)
        {
            MyLogger.Error("Failed to get weapon attach transform");
        }
        
        ChangeWeapons();
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
        if (_selectedWeaponIndex != _input.selectedWeapon - 1)
        {
            ChangeWeapons();
        }
        
        if (_input.attack)
        {
            isAttacking = true;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            RaycastHit hit;
            var originMouse = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(originMouse, out hit))
            {
                // visualize hit point
                //MakeCursorMarker(hit.point);
                
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

    private void ChangeWeapons()
    {
        if (_input.selectedWeapon > weaponInventory.Length)
        {
            MyLogger.Error("Selected weapon outside of inventory range");
            return;
        }

        _selectedWeaponIndex = _input.selectedWeapon - 1;

        // Deactivate current gun
        currentGun.gameObject.SetActive(false);
        
        // Activate new gun (? )
        weaponInventory[_selectedWeaponIndex].gameObject.SetActive(true);
        currentGun = weaponInventory[_selectedWeaponIndex].GetComponent<Gun>();
        
        // Align new gun with attachtransform
        Vector3 difference = currentGun.attachTransform.position - _weaponAttachTransform.transform.position;
        currentGun.transform.position -= difference;
        currentGun.transform.SetParent(_weaponAttachTransform, true);
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
