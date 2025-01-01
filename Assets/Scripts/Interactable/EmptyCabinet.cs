using UnityEngine;

public class EmptyCabinet : PlacableInteractable
{
    public void PlaceObject(GameObject obj)
    {
        placedObject = obj;
        obj.transform.SetParent(placementPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            GameObject carriedObject = playerInteraction.CarriedObject;

            if (carriedObject != null)
            {
                if (placedObject != null)
                {
                    IInteractableItem carriedItem = carriedObject.GetComponent<IInteractableItem>();

                    if (carriedItem != null)
                    {
                        carriedItem.InteractWith(placedObject, this);
                        playerInteraction.isCarrying = false;
                        playerInteraction.animator.SetBool("isCarry", false);
                        if (playerInteraction.CarriedObject == null)
                        {
                            playerInteraction.isCarrying = false;
                            playerInteraction.animator.SetBool("isCarry", false);
                        }
                    }
                    else
                    {
                        Debug.Log("Cannot interact these items.");
                    }
                }
                else
                {
                 
                    base.Interact(player);
                    Debug.Log("Placed object on cabinet.");
                }
            }
            else
            {
              
                if (placedObject != null)
                {
                    
                    playerInteraction.PickUpObject(placedObject);
                    placedObject = null;
                    Debug.Log("Picked up object from cabinet.");
                }
                else
                {
                    Debug.Log("There is nothing on the cabinet.");
                }
            }
        }
    }

    public override bool CanInteract(GameObject player)
        {
            return true; 
        }
    }


