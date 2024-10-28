using UnityEngine;

/// <summary>
/// Oyuncunun animasyon parametrelerini y�netir.
/// </summary>
[RequireComponent(typeof(Animator), typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movementController;

    private void Awake()
    {
        // Animator ve PlayerMovement bile�enlerini al
        animator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    /// <summary>
    /// Oyuncunun hareket verilerine g�re animasyonlar� g�nceller.
    /// </summary>
    private void UpdateAnimations()
    {
        if (movementController != null)
        {
            // Hareket verilerini al
            Vector2 moveInput = movementController.MoveInput;

            // Hareket olup olmad���n� kontrol et
            bool isWalking = moveInput.sqrMagnitude > 0.01f; // SqrMagnitude daha performansl�d�r
            animator.SetBool("isWalking", isWalking);
        }
    }
}
