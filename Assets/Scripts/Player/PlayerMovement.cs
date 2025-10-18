using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
        if (_inputHandler.move != Vector2.zero)
        {
            Vector3 adjustedDirection = new Vector3(_inputHandler.move.x, 0f, _inputHandler.move.y);
            if(!_playerCombat.isAttacking)
                gameObject.transform.forward = adjustedDirection;
            // Vector3 movement = new Vector3(adjustedDirection.x * moveSpeed * Time.deltaTime, gravity * Time.deltaTime,
            //     adjustedDirection.z * moveSpeed * Time.deltaTime);
            // _controller.Move(movement);
            _controller.Move(adjustedDirection * moveSpeed * Time.deltaTime);
        }

        _controller.Move(new Vector3(0f, gravity * Time.deltaTime, 0f));
    }
}
