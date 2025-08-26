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

    public Vector2 move;

    public bool attack;
    public bool reload;

    public TMP_Text debugText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _reloadAction = _playerInput.actions["Reload"];

        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _attackAction.performed += OnAttack;
        _attackAction.canceled += OnAttack;
        _reloadAction.performed += OnReload;
        _reloadAction.canceled += OnReload;
    }

    void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _attackAction.performed -= OnAttack;
        _attackAction.canceled -= OnAttack;
        _reloadAction.performed -= OnReload;
        _reloadAction.canceled -= OnReload;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugText)
        {
            debugText.text = $"Move: {move}\nAttack: {attack}\nReload: {reload}";;
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
}
