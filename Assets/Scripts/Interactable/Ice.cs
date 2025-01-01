using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Carryable, IInteractableItem
{
    public void InteractWith(GameObject target, EmptyCabinet cabinet)
    {
        MojitoGlass glass = target.GetComponent<MojitoGlass>();
        if (glass != null)
        {
            if (!glass.HasIce)
        {
            glass.AddIce();
            Debug.Log("Ice added to the glass.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Glass already has ice.");
        }
        }
    }
    public override void OnPickUp()
    {
        base.OnPickUp();
        Debug.Log("Picked up a ice.");
    }

    public override void OnDrop()
    {
        base.OnDrop();
        Debug.Log("Dropped the ice.");
    }
}

