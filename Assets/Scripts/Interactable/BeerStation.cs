using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerStation : PlacableInteractable
{
    public override void Interact(GameObject player)
    {
        Debug.Log("Interacted with the BeerStation.");

        // If the player is carrying a clean, empty beer glass, fill it
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null && playerInteraction.CarriedObject != null)
        {
            BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
            if (beerGlass != null)
            {
                if (!beerGlass.IsDirty && !beerGlass.IsFilled)
                {
                    // Fill the beer glass
                    beerGlass.Fill();
                }
                else if (beerGlass.IsDirty)
                {
                    Debug.Log("The beer glass is dirty. Clean it first.");
                }
                else if (beerGlass.IsFilled)
                {
                    Debug.Log("The beer glass is already filled.");
                }
            }
            else
            {
                Debug.Log("You need to carry a beer glass to fill it.");
            }
        }
        else
        {
            Debug.Log("You are not carrying anything.");
        }
    }

    public override bool CanInteract(GameObject player)
    {
        // The player can interact with the beer tap if they're carrying a clean, empty beer glass
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null && playerInteraction.CarriedObject != null)
        {
            BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
            return beerGlass != null && !beerGlass.IsDirty && !beerGlass.IsFilled;
        }
        return false;
    }
}