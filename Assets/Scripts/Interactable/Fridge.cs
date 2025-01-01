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
                GameObject IceObj = Instantiate(IcePrefab);
                Ice Ice = IceObj.GetComponent<Ice>();
                if (Ice != null)
                {
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
                Ice Ice = playerInteraction.CarriedObject.GetComponent<Ice>();
                if (Ice != null)
                {
                    Destroy(playerInteraction.CarriedObject);

                    playerInteraction.CarriedObject = null;
                    playerInteraction.isCarrying = false;
                    playerInteraction.animator.SetBool("isCarry", false);
                    Debug.Log("Placed a Ice into the cabinet.");
                }
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                if (mojitoGlass != null && !mojitoGlass.HasIce)
                {
                    mojitoGlass.AddIce();

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
            if (playerInteraction.CarriedObject.GetComponent<MojitoGlass>() !=null)
            {
                return true;
            }
            if (playerInteraction.CarriedObject.GetComponent<Ice>() != null)
            {
                return true;
            }
        }
       
        return false;
    }


}