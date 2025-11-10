using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove = true;
    
    public float moveSpeed = 10.0f;
    public float gravity = -9.8f;
    public float gravityMultiplier = 1.0f;

    private CharacterController _controller;
    private InputHandler _inputHandler;
    private PlayerCombat _playerCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _controller = GetComponent <CharacterController>();
        _playerCombat = GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        if (CanMove)
        {
            if (_inputHandler.move != Vector2.zero)
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
}
