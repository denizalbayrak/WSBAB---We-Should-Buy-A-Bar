using UnityEngine;

/// <summary>
/// Oyuncunun animasyon parametrelerini yönetir.
/// </summary>
[RequireComponent(typeof(Animator), typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on Player!");
        }
    }

    /// <summary>
    /// Sets the animation state for filling beer.
    /// </summary>
    /// <param name="isFilling">Whether the player is filling beer or not.</param>
    public void SetFillingBeer(bool isFilling)
    {
        if (animator != null)
        {
            animator.SetBool("isFillingBeer", isFilling);
        }
    }

    /// <summary>
    /// Triggers the beer filling animation.
    /// </summary>
    public void TriggerFillingBeer()
    {
        if (animator != null)
        {
            animator.SetTrigger("FillBeer");
        }
    }

    /// <summary>
    /// Sets the animation state for washing glass.
    /// </summary>
    /// <param name="isWashing">Whether the player is washing a glass or not.</param>
    public void SetWashingGlass(bool isWashing)
    {
        if (animator != null)
        {
            animator.SetBool("isFillingBeer", isWashing);
        }
    }

    /// <summary>
    /// Sets the animation state for carrying objects.
    /// </summary>
    /// <param name="isCarrying">Whether the player is carrying an object or not.</param>
    public void SetCarrying(bool isCarrying)
    {
        if (animator != null)
        {
            animator.SetBool("isCarry", isCarrying);
        }
    }

    /// <summary>
    /// Sets the walking animation state.
    /// </summary>
    /// <param name="isWalking">Whether the player is walking or not.</param>
    public void SetWalking(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }
}
