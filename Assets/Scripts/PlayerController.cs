using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Rigidbody rb;
    public float speed = 5f;
    public float rotationSpeed = 720f; // Derece cinsinden dönüþ hýzý

    private Animator animator; // Animator referansý

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();

        moveAction = inputActions.Player.Move;

        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;

        animator = GetComponent<Animator>(); // Animator bileþenini al
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
        // Hareket iþlemleri
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 newPosition = transform.position + movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Oyuncunun baktýðý yönü hareket yönüne doðru yumuþak bir þekilde çevir
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }

        // Animasyonlarý güncelle
        UpdateAnimations(movement);
    }

    /// <summary>
    /// Animasyon parametrelerini günceller.
    /// </summary>
    /// <param name="movement">Hareket vektörü.</param>
    private void UpdateAnimations(Vector3 movement)
    {
        if (animator != null)
        {
            bool isWalking = movement.magnitude > 0.1f;
            animator.SetBool("isWalking", isWalking);
        }
    }
}
