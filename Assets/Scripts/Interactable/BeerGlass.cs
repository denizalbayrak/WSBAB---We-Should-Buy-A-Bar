using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerGlass : Carryable
{
    public bool IsDirty = true; // Initial state of the beer glass
    public bool IsFilled = false; // Whether the glass is filled with beer

    public void Clean()
    {
        IsDirty = false;
        // Update the beer glass's appearance to look clean
        // For example, change material or texture
        // Optionally, play a sound or particle effect
    }

    public void Fill()
    {
        if (!IsDirty)
        {
            IsFilled = true;
            // Update the beer glass's appearance to show it's filled
            // For example, add liquid mesh or particle effect
            Debug.Log("Filled the beer glass with beer.");
        }
        else
        {
            Debug.Log("Cannot fill a dirty beer glass.");
        }
    }

    public override void OnPickUp()
    {
        // Specific behavior when the beer glass is picked up
        Debug.Log("Picked up a beer glass.");
    }

    public override void OnDrop()
    {
        // Specific behavior when the beer glass is dropped
        Debug.Log("Dropped the beer glass.");
    }
}