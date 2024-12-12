using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateCrate : PlacableInteractable
{

    [Header("Cabinet Settings")]
    [Tooltip("References to the chocolate objects inside the cabinet.")]
    public GameObject chocolatePrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {

                // Instantiate a new chocolate and give it to the player
                GameObject chocolateObj = Instantiate(chocolatePrefab);
                Chocolate chocolate = chocolateObj.GetComponent<Chocolate>();
                if (chocolate != null)
                {
                    // Have the player pick up the wine glass
                    playerInteraction.PickUpObject(chocolateObj);

                    Debug.Log("Picked up a chocolate from the crate.");
                }
                else
                {
                    Debug.LogError("The chocolate does not have a chocolate component.");
                    Destroy(chocolateObj);
                }
            }
            else
            {
                // Player is carrying something
                // Check if it's a clean, empty wine glass
                Chocolate chocolate = playerInteraction.CarriedObject.GetComponent<Chocolate>();
                if (chocolate != null)
                {
                    Destroy(playerInteraction.CarriedObject);
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    Debug.Log("Placed a chocolate into the cabinet.");
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
            if (playerInteraction.CarriedObject.GetComponent<Lime>() != null)
            {
                return true;
            }
        }


        return false;
    }


}