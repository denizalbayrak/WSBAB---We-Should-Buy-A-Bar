using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : PlacableInteractable
{

    [Header("Cabinet Settings")]
    [Tooltip("References to the Ice objects inside the cabinet.")]
    public GameObject IcePrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {

                // Instantiate a new Ice and give it to the player
                GameObject IceObj = Instantiate(IcePrefab);
                Ice Ice = IceObj.GetComponent<Ice>();
                if (Ice != null)
                {
                    // Have the player pick up the wine glass
                    playerInteraction.PickUpObject(IceObj);

                    Debug.Log("Picked up a Ice from the crate.");
                }
                else
                {
                    Debug.LogError("The Ice does not have a Ice component.");
                    Destroy(IceObj);
                }
            }
            else
            {
                // Player is carrying something
                // Check if it's a clean, empty wine glass
                Ice Ice = playerInteraction.CarriedObject.GetComponent<Ice>();
                if (Ice != null)
                {

                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    Debug.Log("Placed a Ice into the cabinet.");
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
            if (playerInteraction.CarriedObject == null)
            {
                return true;
            }
        }
       
        return false;
    }


}