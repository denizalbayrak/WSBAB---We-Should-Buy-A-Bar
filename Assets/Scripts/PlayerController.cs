using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Rigidbody rb;
    public float speed = 5f;
    public float rotationSpeed = 720f; // Derece cinsinden d�n�� h�z�

    private Animator animator; // Animator referans�

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();

        moveAction = inputActions.Player.Move;

        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;

        animator = GetComponent<Animator>(); // Animator bile�enini al
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the player!");
        }
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
        // Hareket i�lemleri
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 newPosition = transform.position + movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Oyuncunun bakt��� y�n� hareket y�n�ne do�ru yumu�ak bir �ekilde �evir
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }

        // Animasyonlar� g�ncelle
        UpdateAnimations(movement);
    }

    /// <summary>
    /// Animasyon parametrelerini g�nceller.
    /// </summary>
    /// <param name="movement">Hareket vekt�r�.</param>
    private void UpdateAnimations(Vector3 movement)
    {
        if (animator != null)
        {
            bool isWalking = movement.magnitude > 0.1f;
            animator.SetBool("isWalking", isWalking);
        }
    }
}
