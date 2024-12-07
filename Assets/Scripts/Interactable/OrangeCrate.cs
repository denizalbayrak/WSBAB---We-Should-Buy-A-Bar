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
        Debug.Log("00");
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {

                // Instantiate a new orange and give it to the player
                GameObject orangeObj = Instantiate(orangePrefab);
                Orange orange = orangeObj.GetComponent<Orange>();
                Debug.Log("1");
                if (orange != null)
                {
                    // Have the player pick up the mimosa glass
                    playerInteraction.PickUpObject(orangeObj);

                    Debug.Log("Picked up a orange from the crate.");
                    Debug.Log("2");
                }
                else
                {
                    Debug.LogError("The orange does not have a Orange component.");
                    Debug.Log("3");
                    Destroy(orangeObj);
                }
            }
            else
            {
                // Player is carrying something
                // Check if it's a clean, empty mimosa glass
                Debug.Log("4");
                Orange orange = playerInteraction.CarriedObject.GetComponent<Orange>();
                if (orange != null)
                {

                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    Debug.Log("Placed a orange into the cabinet.");
                }
            }
        }
        else
        {
            Debug.Log("5");
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
