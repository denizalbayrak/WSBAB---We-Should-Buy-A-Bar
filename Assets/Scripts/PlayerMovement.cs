using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Oyuncunun hareketini yönetir.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Rigidbody rb;
    public float speed = 5f;
    public float rotationSpeed = 720f; // Derece cinsinden dönüþ hýzý
    PhotonView view;
    /// <summary>
    /// Hareket giriþlerini dýþa açar.
    /// </summary>
    public Vector2 MoveInput => moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();

        moveAction = inputActions.Player.Move;
        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;
        view = GetComponent<PhotonView>();
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
        if (view.IsMine)
        {
            Move();
    }
    }

    /// <summary>
    /// Oyuncunun hareket ve rotasyon iþlemlerini yönetir.
    /// </summary>
    private void Move()
    {
        Vector3 movement = CalculateMovement();
        ApplyMovement(movement);
        RotatePlayer(movement);
    }

    /// <summary>
    /// Hareket vektörünü hesaplar.
    /// </summary>
    private Vector3 CalculateMovement()
    {
        // Normalize edilmemiþ hareket vektörü
        return new Vector3(moveInput.x, 0, moveInput.y);
    }

    /// <summary>
    /// Hareketi uygular ve yeni pozisyonu hesaplar.
    /// </summary>
    private void ApplyMovement(Vector3 movement)
    {
        if (movement.sqrMagnitude > 0.01f) // Hareketin küçük olup olmadýðýný kontrol eder
        {
            movement.Normalize();
            Vector3 newPosition = transform.position + movement * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    /// <summary>
    /// Oyuncuyu hareket yönüne doðru döndürür.
    /// </summary>
    private void RotatePlayer(Vector3 movement)
    {
        if (movement.sqrMagnitude > 0.01f) // Hareket yoksa döndürme iþlemi yapýlmaz
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
