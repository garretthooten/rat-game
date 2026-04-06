using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _meleeAction;
    private InputAction _reloadAction;
    private InputAction _weaponSelectAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _interactAction;
    private InputAction _pauseAction;

    public Vector2 move;

    public bool attack;
    public bool melee;
    public bool reload;
    public bool jump;
    public bool dash;
    public bool interact;
    public bool pause;

    public int selectedWeapon = 1;

    public TMP_Text debugText;

    // subscribe actions
    public event Action OnMeleeInput;
    public event Action OnMeleeInputCanceled;
    public event Action OnDashInput;
    public event Action OnInteractInput;
    public event Action OnPauseInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _meleeAction = _playerInput.actions["Melee"];
        _reloadAction = _playerInput.actions["Reload"];
        _weaponSelectAction = _playerInput.actions["Weapon Select"];
        _jumpAction = _playerInput.actions["Jump"];
        _dashAction = _playerInput.actions["Dash"];
        _interactAction = _playerInput.actions["Interact"];
        _pauseAction = _playerInput.actions["Pause"];

        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _attackAction.performed += OnAttack;
        _attackAction.canceled += OnAttack;
        _meleeAction.performed += OnMelee;
        _meleeAction.canceled += OnMelee;
        _reloadAction.performed += OnReload;
        _reloadAction.canceled += OnReload;
        // makethis a consumabel input?
        _weaponSelectAction.performed += OnWeaponSelect;
        _jumpAction.performed += OnJump;
        _jumpAction.canceled += OnJump;
        _dashAction.performed += OnDash;
        _dashAction.canceled += OnDash;
        _interactAction.performed += OnInteract;
        _interactAction.canceled += OnInteract;
        _pauseAction.performed += OnPause;
        _pauseAction.canceled += OnPause;
    }

    void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _attackAction.performed -= OnAttack;
        _attackAction.canceled -= OnAttack;
        _meleeAction.performed -= OnMelee;
        _meleeAction.canceled -= OnMelee;
        _reloadAction.performed -= OnReload;
        _reloadAction.canceled -= OnReload;
        _weaponSelectAction.performed -= OnWeaponSelect;
        _jumpAction.performed -= OnJump;
        _jumpAction.canceled -= OnJump;
        _dashAction.performed -= OnDash;
        _dashAction.canceled -= OnDash;
        _interactAction.performed -= OnInteract;
        _interactAction.canceled -= OnInteract;
        _pauseAction.performed -= OnPause;
        _pauseAction.canceled -= OnPause;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugText)
        {
            debugText.text = $"Pause: {pause}\nMove: {move}\nAttack: {attack}\nMelee: {melee}\nReload: {reload}\nJump: {jump}\nDash: {dash}\nInteract: {interact}";;
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        move = _moveAction.ReadValue<Vector2>();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            attack = true;
        else if (context.phase == InputActionPhase.Canceled)
            attack = false;
    }

    void OnMelee(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            melee = true;
            OnMeleeInput?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            melee = false;
            OnMeleeInputCanceled?.Invoke();
        }
    }

    void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            reload = true;
        else if (context.phase == InputActionPhase.Canceled)
            reload = false;
    }

    void OnWeaponSelect(InputAction.CallbackContext context)
    {
        var control = context.control;
        if (control.device is Keyboard keyboard)
        {
            if (control == keyboard.digit1Key) selectedWeapon = 1;
            else if (control == keyboard.digit2Key) selectedWeapon = 2;
            else if (control == keyboard.digit3Key) selectedWeapon = 3;
            else if (control == keyboard.digit4Key) selectedWeapon = 4;
            else if (control == keyboard.digit5Key) selectedWeapon = 5;
            else if (control == keyboard.digit6Key) selectedWeapon = 6;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            jump = true;
        else if (context.phase == InputActionPhase.Canceled)
            jump = false;
    }
    
    void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            dash = true;
            OnDashInput?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
            dash = false;
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            interact = true;
            OnInteractInput?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
            interact = false;
    }
    
    void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            pause = true;
            OnPauseInput?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
            pause = false;
    }
}
