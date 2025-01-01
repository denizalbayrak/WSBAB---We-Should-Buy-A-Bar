using UnityEngine;


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


    public void SetFillingBeer(bool isFilling)
    {
        if (animator != null)
        {
            animator.SetBool("isFillingBeer", isFilling);
        }
    } 
    public void SetChopping(bool isFilling)
    {
        if (animator != null)
        {
            animator.SetBool("isChopping", isFilling);
        }
    }  
    public void SetBlending(bool isFilling)
    {
        if (animator != null)
        {
            animator.SetBool("isBlending", isFilling);
        }
    }

 
    public void TriggerFillingBeer()
    {
        if (animator != null)
        {
            animator.SetTrigger("Chop");
        }
    }
    public void TriggerChopping()
    {
        if (animator != null)
        {
            animator.SetTrigger("Chop");
        }
    } public void TriggerBlending()
    {
        if (animator != null)
        {
            animator.SetTrigger("Blend");
        }
    }

    public void SetWashingGlass(bool isWashing)
    {
        if (animator != null)
        {
            animator.SetBool("isFillingBeer", isWashing);
        }
    }

    public void SetCarrying(bool isCarrying)
    {
        if (animator != null)
        {
            animator.SetBool("isCarry", isCarrying);
        }
    }

    public void SetWalking(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }
}
