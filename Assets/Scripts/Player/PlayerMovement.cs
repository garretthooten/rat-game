using UnityEngine;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove = true;
    
    public float moveSpeed = 10.0f;
    public float gravity = -9.8f;
    public float gravityMultiplier = 1.0f;

    [Header("Dash Configuration")]
    public int maxDashes = 2;
    [SerializeField] private int _currentDashes;
    public bool isDashing = false;
    public float dashDistance = 2.0f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 1.0f;
    private float _timeOfLastDash;
    private Vector3 dashDirection;
    public event Action OnDashPerformed;
    public event Action OnDashCanceled;
    

    private CharacterController _controller;
    private InputHandler _inputHandler;
    private PlayerCombat _playerCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _controller = GetComponent <CharacterController>();
        _playerCombat = GetComponent<PlayerCombat>();

        _currentDashes = maxDashes;
    }

    void OnEnable()
    {
        if(_inputHandler)
            _inputHandler.OnDashInput += Dash;
        else
            Debug.LogError("No input handler");
    }

    void OnDisable()
    {
        _inputHandler.OnDashInput -= Dash;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        if (CanMove)
        {
            if (isDashing)
            {
                float dashSpeed = dashDistance / dashDuration;
                move = dashDirection * dashSpeed;
            }
            
            else if (_inputHandler.move != Vector2.zero)
            {
                Vector3 adjustedDirection = new Vector3(_inputHandler.move.x, 0f, _inputHandler.move.y);
                if (!_playerCombat.isAttacking)
                    gameObject.transform.forward = adjustedDirection;
                move += adjustedDirection * moveSpeed;
            }
        }

        move.y = gravity * gravityMultiplier;
        _controller.Move(move * Time.deltaTime);
        
        // temp debugging
        //Vector3 move = new Vector3(_inputHandler.move.x, 0f, _inputHandler.move.y);
        //_controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void Dash()
    {
        Debug.Log("Got dash event");
        if (CanMove && _currentDashes > 0)
        {
            Vector3 direction;
            if (_inputHandler.move == Vector2.zero)
                direction = transform.forward;
            else
            {
                direction = new Vector3(_inputHandler.move.x, 0f, _inputHandler.move.y);
            }

            StartCoroutine(PerformDash(direction));
        }
    }

    private IEnumerator PerformDash(Vector3 direction)
    {
        _currentDashes--;
        dashDirection = direction;
        float dashSpeed = dashDistance / dashDuration;
        Debug.Log($"Dash beginning with dashSpeed {dashSpeed}");
        OnDashPerformed?.Invoke();
        //yield return null;
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        OnDashCanceled?.Invoke();
        StartCoroutine(StartDashCooldown());
    }

    private IEnumerator StartDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        _currentDashes++;
    }
}
