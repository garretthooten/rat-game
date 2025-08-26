using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    private  InputHandler _input;
    private bool _lastAttack;
    public bool isAttacking;
    public Gun currentGun;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.attack)
        {
            Logger.Info($"Attack pressed");
            isAttacking = true;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            RaycastHit hit;
            var originMouse = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(originMouse, out hit))
            {
                Vector3 playerToHitDirection = hit.point - transform.position;
                Vector3 normalizedDirection = playerToHitDirection.normalized;
                gameObject.transform.forward = new Vector3(normalizedDirection.x, 0, normalizedDirection.z);
                
                if (currentGun)
                {
                    Logger.Info($"Pulling gun trigger");
                    currentGun.Fire(hit.point);
                }
            }
            
        }
        else
        {
            if (!_input.attack && _lastAttack && currentGun)
                Logger.Info($"Releasing gun trigger");
                currentGun.ReleaseTrigger();
            isAttacking = false;
        }

        _lastAttack = _input.attack;
    }
}
