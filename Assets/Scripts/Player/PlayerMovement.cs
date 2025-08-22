using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;

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
        if (!_playerCombat.isAttacking)
        {
            if (_inputHandler.move != Vector2.zero)
            {
                Vector3 adjustedDirection = new Vector3(_inputHandler.move.x, 0f, _inputHandler.move.y);
                gameObject.transform.forward = adjustedDirection;
                _controller.Move(adjustedDirection * Time.deltaTime * moveSpeed);
            }
        }
    }
}
