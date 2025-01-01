using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Rigidbody rb;
    public float speed = 5f;
    public float rotationSpeed = 720f; 


    public Vector2 MoveInput => moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();

        moveAction = inputActions.Player.Move;
        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        Vector3 movement = CalculateMovement();
        ApplyMovement(movement);
        RotatePlayer(movement);
    }

    private Vector3 CalculateMovement()
    {
      
        return new Vector3(moveInput.x, 0, moveInput.y);
    }

    private void ApplyMovement(Vector3 movement)
    {
        if (movement.sqrMagnitude > 0.01f) 
        {
            movement.Normalize();
            Vector3 newPosition = transform.position + movement * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    private void RotatePlayer(Vector3 movement)
    {
        if (movement.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}
