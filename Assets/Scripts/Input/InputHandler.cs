using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _reloadAction;
    private InputAction _weaponSelectAction;
    private InputAction _jumpAction;

    public Vector2 move;

    public bool attack;
    public bool reload;
    public bool jump;

    public int selectedWeapon = 1;

    public TMP_Text debugText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _reloadAction = _playerInput.actions["Reload"];
        _weaponSelectAction = _playerInput.actions["Weapon Select"];
        _jumpAction = _playerInput.actions["Jump"];

        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _attackAction.performed += OnAttack;
        _attackAction.canceled += OnAttack;
        _reloadAction.performed += OnReload;
        _reloadAction.canceled += OnReload;
        // makethis a consumabel input?
        _weaponSelectAction.performed += OnWeaponSelect;
        _jumpAction.performed += OnJump;
        _jumpAction.canceled += OnJump;
    }

    void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _attackAction.performed -= OnAttack;
        _attackAction.canceled -= OnAttack;
        _reloadAction.performed -= OnReload;
        _reloadAction.canceled -= OnReload;
        _weaponSelectAction.performed -= OnWeaponSelect;
        _jumpAction.performed -= OnJump;
        _jumpAction.canceled -= OnJump;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugText)
        {
            debugText.text = $"Move: {move}\nAttack: {attack}\nReload: {reload}\nJump: {jump}";;
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
}
