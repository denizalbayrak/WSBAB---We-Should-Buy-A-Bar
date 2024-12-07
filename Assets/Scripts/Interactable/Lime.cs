using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lime : Carryable, IInteractableItem, IChoppable
{
    public enum LimeState
    {
        FullLime,
        ChoppedLime
    }
    public LimeState CurrentState = LimeState.FullLime;
    public GameObject fullVisual; 
    public GameObject choppedVisual;
    public bool IsFull => CurrentState == LimeState.FullLime;
    public bool IsChopped => CurrentState == LimeState.ChoppedLime;
    private void Start()
    {
        UpdateVisuals();
    }
   

public void Full()
    {
        CurrentState = LimeState.FullLime;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The lime is now full.");
    }

    public void Chop()
    {
        CurrentState = LimeState.ChoppedLime;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The lime is now chop.");
    }
    public void UpdateVisuals()
    {
        if (fullVisual != null)
        {
            fullVisual.SetActive(CurrentState == LimeState.FullLime);           
        }


        if (choppedVisual != null)
            choppedVisual.SetActive(CurrentState == LimeState.ChoppedLime);

    }
    public void InteractWith(GameObject target, EmptyCabinet cabinet)
    {
        MojitoGlass glass = target.GetComponent<MojitoGlass>();
        if (glass != null)
        {
            if (CurrentState == LimeState.ChoppedLime)
        {
            if (!glass.HasChocolate)
            {
                glass.AddLime();
                Debug.Log("Lime added to the glass.");
                Destroy(gameObject); // Lime'ý yok et
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
