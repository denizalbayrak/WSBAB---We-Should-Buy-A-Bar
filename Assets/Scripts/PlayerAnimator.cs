using Photon.Pun;
using UnityEngine;

/// <summary>
/// Oyuncunun animasyon parametrelerini yönetir.
/// </summary>
[RequireComponent(typeof(Animator), typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
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
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetBool("isFillingBeer", isFilling);
            }
        }
    }
    public void SetChopping(bool isFilling)
    {
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetBool("isChopping", isFilling);
            }
        }
    }
    public void SetBlending(bool isFilling)
    {
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetBool("isBlending", isFilling);
            }
        }
    }

    /// <summary>
    /// Triggers the beer filling animation.
    /// </summary>
    public void TriggerFillingBeer()
    {
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetTrigger("Chop");
            }
        }
    }
    public void TriggerChopping()
    {
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetTrigger("Chop");
            }
        }
    }
    public void TriggerBlending()
    {
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetTrigger("Blend");
            }
        }
    }

    /// <summary>
    /// Sets the animation state for washing glass.
    /// </summary>
    /// <param name="isWashing">Whether the player is washing a glass or not.</param>
    public void SetWashingGlass(bool isWashing)
    {
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetBool("isFillingBeer", isWashing);
            }
        }
    }

    /// <summary>
    /// Sets the animation state for carrying objects.
    /// </summary>
    /// <param name="isCarrying">Whether the player is carrying an object or not.</param>
    public void SetCarrying(bool isCarrying)
    {
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetBool("isCarry", isCarrying);
            }
        }
    }

    /// <summary>
    /// Sets the walking animation state.
    /// </summary>
    /// <param name="isWalking">Whether the player is walking or not.</param>
    public void SetWalking(bool isWalking)
    {
        if (view.IsMine)
        {
            if (animator != null)
            {
                animator.SetBool("isWalking", isWalking);
            }
        }
    }
}
