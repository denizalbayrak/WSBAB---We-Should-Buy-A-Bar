using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : PlacableInteractable
{
    public override void Interact(GameObject player)
    {
        // Specific interaction behavior
        Debug.Log("Interacted with the sink.");
    }

//    PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
//        if (playerInteraction != null && playerInteraction.CarriedObject != null)
//        {
//            // Check if the carried object is a dirty beer glass
//            BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
//            if (beerGlass != null && beerGlass.IsDirty)
//            {
//                // Clean the beer glass
//                beerGlass.Clean();
//                Debug.Log("Cleaned the beer glass.");
//            }
//            else
//{
//    Debug.Log("You are not carrying a dirty beer glass.");
//}
//        }
//        else
//{
//    Debug.Log("You are not carrying anything.");
//}
//    }

//    public override bool CanInteract(GameObject player)
//{
//    // The player can interact with the sink if they're carrying a dirty beer glass
//    PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
//    if (playerInteraction != null && playerInteraction.CarriedObject != null)
//    {
//        BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
//        return beerGlass != null && beerGlass.IsDirty;
//    }
//    return false;
//}
}