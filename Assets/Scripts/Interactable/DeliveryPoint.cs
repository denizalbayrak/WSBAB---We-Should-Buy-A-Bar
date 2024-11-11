using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : PlacableInteractable
{
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Player is carrying an object: Place it on the cabinet
                if (placedObject == null)
                {
                    base.Interact(player);
                    Destroy(placedObject);
                    Debug.Log("Placed object on DeliveryPoint.");
                }
                else
                {
                    Debug.Log("Cannot place object on cabinet. It's already occupied.");
                }
            }

        }
        else
        {
            Debug.Log("Cannot interact with the cabinet.");
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Can interact if carrying an object and the cabinet is empty
                return placedObject == null;
            }
            else
            {
                // Can interact if not carrying anything and the cabinet has an object
                return placedObject != null;
            }
        }
        return false;
    }
}
