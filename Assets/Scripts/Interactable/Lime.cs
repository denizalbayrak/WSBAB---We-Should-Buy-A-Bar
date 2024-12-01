using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lime : Carryable
{
    public enum LimeState
    {
        FullLime,
        ChoppedLime
    }
    public LimeState CurrentState = LimeState.FullLime;
    public GameObject fullVisual; 
    public GameObject choppedVisual; 
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
    private void UpdateVisuals()
    {
        if (fullVisual != null)
        {
            fullVisual.SetActive(CurrentState == LimeState.FullLime);           
        }


        if (choppedVisual != null)
            choppedVisual.SetActive(CurrentState == LimeState.ChoppedLime);

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
