using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : PlacableInteractable
{

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            // Player is carrying something
            if (playerInteraction.CarriedObject != null)
            {
                    Destroy(playerInteraction.CarriedObject);
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    Debug.Log("Object trashed");
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
                return true;
            }
        }


        return false;
    }


}