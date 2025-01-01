using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chocolate : Carryable, IInteractableItem, IChoppable
{
    public enum ChocolateState
    {
        FullChocolate,
        ChoppedChocolate
    }
    public ChocolateState CurrentState = ChocolateState.FullChocolate;
    public GameObject fullVisual;
    public GameObject choppedVisual;
    public bool IsFull => CurrentState == ChocolateState.FullChocolate;
    public bool IsChopped => CurrentState == ChocolateState.ChoppedChocolate;
    private void Start()
    {
        UpdateVisuals();
    }


    public void Full()
    {
        CurrentState = ChocolateState.FullChocolate;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The chocolate is now full.");
    }

    public void Chop()
    {
        CurrentState = ChocolateState.ChoppedChocolate;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The chocolate is now chop.");
    }
    public void UpdateVisuals()
    {
        if (fullVisual != null)
        {
            fullVisual.SetActive(CurrentState == ChocolateState.FullChocolate);
        }


        if (choppedVisual != null)
            choppedVisual.SetActive(CurrentState == ChocolateState.ChoppedChocolate);

    }
    public void InteractWith(GameObject target, EmptyCabinet cabinet)
    {
        WhiskeyGlass glass = target.GetComponent<WhiskeyGlass>();
        if (glass != null)
        {
            if (CurrentState == ChocolateState.ChoppedChocolate)
            {
                if (!glass.HasChocolate)
                {
                    glass.AddChocolate();
                    Debug.Log("chocolate added to the glass.");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Glass already has chocolate.");
                }
            }
            else
            {
                Debug.Log("You need to chop the chocolate first.");
            }
        }
    }
    public override void OnPickUp()
    {
        base.OnPickUp();
        Debug.Log("Picked up a chocolate.");
    }

    public override void OnDrop()
    {
        base.OnDrop();
        Debug.Log("Dropped the chocolate.");
    }
}
