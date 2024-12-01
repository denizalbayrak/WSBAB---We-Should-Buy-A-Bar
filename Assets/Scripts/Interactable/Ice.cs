using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Carryable
{ 
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

