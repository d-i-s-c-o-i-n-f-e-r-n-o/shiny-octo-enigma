using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManipulator : MonoBehaviour
{
    public float moveSpeed = 5f;
    Rigidbody2D rb;
    PlayerInput input;
    //private Vector2 movementInput;

    private void Awake()
    {
        input = new PlayerInput();
        input.Player.Attack.performed += context => Attack();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        Vector2 direction = input.Player.Move.ReadValue<Vector2>();
        Move(direction);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    Debug.Log($"OnMove Called: {context.started} {context.canceled}");  // Check when OnMove is called
    //    if (context.started || context.canceled)
    //    {
    //        movementInput = context.ReadValue<Vector2>();
    //    }
    //}
    void Attack()
    {
        Debug.Log("attack!!");
    }


    void Move(Vector2 movementInput)
    {
        rb.linearVelocity = movementInput.normalized * moveSpeed;
    }
}
