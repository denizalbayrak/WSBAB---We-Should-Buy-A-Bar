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

                GameObject chocolateObj = Instantiate(chocolatePrefab);
                Chocolate chocolate = chocolateObj.GetComponent<Chocolate>();
                if (chocolate != null)
                {
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
                Chocolate chocolate = playerInteraction.CarriedObject.GetComponent<Chocolate>();
                if (chocolate != null)
                {
                    Destroy(playerInteraction.CarriedObject);
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
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