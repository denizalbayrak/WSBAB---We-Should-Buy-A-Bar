using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeCrate : PlacableInteractable
{

    [Header("Cabinet Settings")]
    [Tooltip("References to the lime objects inside the cabinet.")]
    public GameObject orangePrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {

                // Instantiate a new orange and give it to the player
                GameObject orangeObj = Instantiate(orangePrefab);
                Orange orange = orangeObj.GetComponent<Orange>();
                if (orange != null)
                {
                    // Have the player pick up the mimosa glass
                    playerInteraction.PickUpObject(orangeObj);

                    Debug.Log("Picked up a orange from the crate.");
                }
                else
                {
                    Debug.LogError("The orange does not have a Orange component.");
                    Destroy(orangeObj);
                    
                }
            }
            else
            {
                // Player is carrying something
                // Check if it's a clean, empty mimosa glass
                Orange orange = playerInteraction.CarriedObject.GetComponent<Orange>();
                if (orange != null)
                {

                    Destroy(playerInteraction.CarriedObject);
                    playerInteraction.CarriedObject = null;
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    // Reset the player's carried object

                    Debug.Log("Placed a orange into the cabinet.");
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
            if (playerInteraction.CarriedObject.GetComponent<Orange>() != null)
            {
                return true;
            }
        }

        return false;
    }


}
