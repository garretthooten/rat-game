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

    public Vector2 move;

    public bool attack;

    public TMP_Text debugText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];

        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _attackAction.performed += OnAttack;
        _attackAction.canceled += OnAttack;
    }

    void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _attackAction.performed -= OnAttack;
        _attackAction.canceled -= OnAttack;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugText)
        {
            debugText.text = $"Move: {move}";
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        move = _moveAction.ReadValue<Vector2>();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        attack = _attackAction.ReadValue<bool>();
    }
}
