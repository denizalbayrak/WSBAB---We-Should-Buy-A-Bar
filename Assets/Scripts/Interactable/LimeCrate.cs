using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimeCrate : PlacableInteractable
{
    [Header("Cabinet Settings")]
    [Tooltip("References to the lime objects inside the cabinet.")]
    public GameObject limePrefab;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {                              
               
                // Instantiate a new lime and give it to the player
                GameObject limeObj = Instantiate(limePrefab);
                Lime lime = limeObj.GetComponent<Lime>();
                if (lime != null)
                {
                    // Have the player pick up the wine glass
                    playerInteraction.PickUpObject(limeObj);

                    Debug.Log("Picked up a lime from the crate.");
                }
                else
                {
                    Debug.LogError("The lime does not have a Lime component.");
                    Destroy(limeObj);
                }
            }
            else
            {
                // Player is carrying something
                // Check if it's a clean, empty wine glass
                Lime lime = playerInteraction.CarriedObject.GetComponent<Lime>();
                if (lime != null)
                {
                   
                    Destroy(playerInteraction.CarriedObject);

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;
                    
                    Debug.Log("Placed a lime into the cabinet.");
                }                
            }
        }
        else
        {
            Debug.Log("Cannot interact with the cabinet.");
        }
    }

    //public override bool CanInteract(GameObject player)
    //{
    //    PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
    //    if (playerInteraction != null)
    //    {
    //        if (playerInteraction.CarriedObject == null)
    //        {
    //            // Can interact if the cabinet has lime glasses to give
    //            return GetAvailableWineGlassCount() > 0;
    //        }
    //        else
    //        {
    //            // Can place a clean, empty wine glass if there's room in the cabinet
    //            WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
    //            return wineGlass != null &&
    //                   wineGlass.CurrentState == WineGlass.GlassState.CleanEmpty &&
    //                   GetAvailableSlotsInCabinet() > 0;
    //        }
    //    }
    //    return false;
    //}

    
}