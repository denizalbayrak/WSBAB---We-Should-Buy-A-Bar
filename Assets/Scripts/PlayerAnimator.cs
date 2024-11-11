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
        // Animator ve PlayerMovement bileşenlerini al
        animator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    /// <summary>
    /// Oyuncunun hareket verilerine göre animasyonları günceller.
    /// </summary>
    private void UpdateAnimations()
    {
        //if (movementController != null)
        //{
        //    // Hareket verilerini al
        //    Vector2 moveInput = movementController.MoveInput;

        //    // Hareket olup olmadığını kontrol et
        //    bool isWalking = moveInput.sqrMagnitude > 0.01f; // SqrMagnitude daha performanslıdır
        //    animator.SetBool("isWalking", isWalking);
        //}
    }
}
