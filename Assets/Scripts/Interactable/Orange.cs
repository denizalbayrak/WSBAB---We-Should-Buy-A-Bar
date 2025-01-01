using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : Carryable, IInteractableItem, IChoppable, IBlendable
{
    public enum OrangeState
    {
        FullOrange,
        ChoppedOrange,
        BlendedOrange,
    }
    public OrangeState CurrentState = OrangeState.FullOrange;
    public GameObject fullVisual;
    public GameObject choppedVisual;
    public GameObject blendedOrangeVisual;
    public bool IsFull => CurrentState == OrangeState.FullOrange;
    public bool IsChopped => CurrentState == OrangeState.ChoppedOrange;
    public bool IsBlendable => CurrentState == OrangeState.ChoppedOrange;
    public bool IsBlended => CurrentState == OrangeState.BlendedOrange;
    private void Start()
    {
        UpdateVisuals();
    }
    public void Full()
    {
        CurrentState = OrangeState.FullOrange;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The orange is now full.");
    }
    public void Blend()
    {
        if (IsBlendable && !IsBlended)
        {
            CurrentState = OrangeState.BlendedOrange;
            UpdateVisuals();
            Debug.Log("Orange has been blended.");
        }
    }
    public void Chop()
    {
        CurrentState = OrangeState.ChoppedOrange;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The orange is now chop.");
    }
    public void UpdateVisuals()
    {
        if (fullVisual != null)        
            fullVisual.SetActive(CurrentState == OrangeState.FullOrange);
        
        if (choppedVisual != null)
            choppedVisual.SetActive(CurrentState == OrangeState.ChoppedOrange);

       
    }
    public void InteractWith(GameObject target, EmptyCabinet cabinet)
    {
        MimosaGlass glass = target.GetComponent<MimosaGlass>();
        if (glass != null)
        {
            if (CurrentState == OrangeState.FullOrange)
            {
                if (!glass.HasOrangeJuice)
                {
                    glass.AddOrangeJuice();
                    Debug.Log("Lime added to the glass.");
                    Destroy(gameObject); 
                }
                else
                {
                    Debug.Log("Glass already has lime.");
                }
            }
            else
            {
                Debug.Log("You need to chop the lime first.");
            }
        }
    }
    public override void OnPickUp()
    {
        base.OnPickUp();
        Debug.Log("Picked up a lime.");
    }

    public override void OnDrop()
    {
        base.OnDrop();
        Debug.Log("Dropped the lime.");
    }
}
