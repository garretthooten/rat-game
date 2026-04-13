using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    private InputHandler _input;
    private PlayerMovement _movement;
    private bool _lastAttack;
    private GameObject _cursorMarker;

    // melee attack stuff
    [Header("Melee Configuration")]
    [SerializeField] private AudioClip _smallMeleeSound;
    [SerializeField] private AudioClip _bigMeleeSound;
    [SerializeField] private float _meleeChargeMoveSpeedFactor = 0.7f;
    [SerializeField] private float _meleeInputDurationWindow = 0.5f;
    [SerializeField] private float _smallMeleeAttackDuration = 0.25f;
    [SerializeField] private float _smallMeleeAttackDamage = 25f; // controlled by what melee weapon you have later
    [SerializeField] private GameObject _smallMeleeAttackHurtbox;
    [SerializeField] private float _bigMeleeAttackDuration = 0.5f;
    [SerializeField] private float _bigMeleeAttackDamage = 50f;
    [SerializeField] private GameObject _bigMeleeAttackHurtbox;
    
    private bool _meleeInputPressed = false;
    private bool _isMeleeAttacking = false;
    private float _timeOfMeleeInputStart;
    private float _timeOfMeleeInputStop;
    [Header("General Configuration")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private int _layerMask;
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
        _movement = GetComponent<PlayerMovement>();
        _audioSource = GetComponent<AudioSource>();
        _layerMask = LayerMask.GetMask("Floor");
        if (!_weaponAttachTransform)
        {
            MyLogger.Error("Failed to get weapon attach transform");
        }
        
        ChangeWeapons();
    }

    void OnEnable()
    {
        _input.OnMeleeInput += StartMeleeAttackInput;
        _input.OnMeleeInputCanceled += StopMeleeAttackInput;
        _smallMeleeAttackHurtbox.SetActive(false);
        _bigMeleeAttackHurtbox.SetActive(false);
    }

    void OnDisable()
    {
        _input.OnMeleeInput -= StartMeleeAttackInput;
        _input.OnMeleeInputCanceled -= StopMeleeAttackInput;
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

    public void AddWeaponAmmo(Gun.Firetype type, int amount)
    {
        foreach(var gun in weaponInventory)
        {
            if(gun.GetComponent<Gun>().TryAddAmmo(type, amount))
            {
                return;
            }
        }
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
        
        if (_input.attack && !_input.melee && !_isMeleeAttacking)
        {
            isAttacking = true;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            RaycastHit hit;
            var originMouse = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(originMouse, out hit, 999f, _layerMask))
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

    private void StartMeleeAttackInput()
    {
        StartCoroutine(MeleeAttackProcess());
    }

    private void StopMeleeAttackInput()
    {
        _timeOfMeleeInputStop = Time.time;
        _meleeInputPressed = false;
    }

    IEnumerator MeleeAttackProcess()
    {
        _timeOfMeleeInputStart = Time.time;
        _movement.ChangeSpeed(_meleeChargeMoveSpeedFactor);
        while(_input.melee)
        {
            yield return null;
        }
        _timeOfMeleeInputStop = Time.time;
        float duration = _timeOfMeleeInputStop - _timeOfMeleeInputStart;
        bool isBigMeleeAttack = (duration > _meleeInputDurationWindow);
        Debug.Log($"Melee attack lifted, duration: {duration}s, isBigMeleeAttack: {isBigMeleeAttack}");
        _isMeleeAttacking = true;
        if(!isBigMeleeAttack)
        {
            _movement.RestoreSpeed();
            _smallMeleeAttackHurtbox.SetActive(true);
            _audioSource.PlayOneShot(_smallMeleeSound);
            yield return new WaitForSeconds(_smallMeleeAttackDuration);
            _smallMeleeAttackHurtbox.SetActive(false);
        }
        else
        {
            _bigMeleeAttackHurtbox.SetActive(true);
            _audioSource.PlayOneShot(_bigMeleeSound);
            yield return new WaitForSeconds(_bigMeleeAttackDuration);
            _movement.RestoreSpeed();
            _bigMeleeAttackHurtbox.SetActive(false);
        }
        _isMeleeAttacking = false;
        
    }
}
