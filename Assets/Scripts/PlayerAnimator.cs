using UnityEngine;

/// <summary>
/// Oyuncunun animasyon parametrelerini yönetir.
/// </summary>
[RequireComponent(typeof(Animator), typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movementController;

    private void Awake()
    {
        // Animator ve PlayerMovement bileþenlerini al
        animator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    /// <summary>
    /// Oyuncunun hareket verilerine göre animasyonlarý günceller.
    /// </summary>
    private void UpdateAnimations()
    {
        if (movementController != null)
        {
            // Hareket verilerini al
            Vector2 moveInput = movementController.MoveInput;

            // Hareket olup olmadýðýný kontrol et
            bool isWalking = moveInput.sqrMagnitude > 0.01f; // SqrMagnitude daha performanslýdýr
            animator.SetBool("isWalking", isWalking);
        }
    }
}
