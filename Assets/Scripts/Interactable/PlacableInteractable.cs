using UnityEngine;

public class PlacableInteractable : Interactable
{
    public Transform placementPoint;
    protected GameObject placedObject; 

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                if (placedObject == null)
                {
                    GameObject carriedObject = playerInteraction.CarriedObject;

                    carriedObject.transform.SetParent(placementPoint);

                    carriedObject.transform.localPosition = Vector3.zero;
                    carriedObject.transform.localRotation = Quaternion.identity;

                    Carryable carryable = carriedObject.GetComponent<Carryable>();
                    if (carryable != null)
                    {
                        carryable.OnDrop();
                    }

                    playerInteraction.CarriedObject = null;
                    playerInteraction.isCarrying = false; 
                    playerInteraction.animator.SetBool("isCarry", false);
                    placedObject = carriedObject;

                    Debug.Log("Placed object on interactable.");
                }
                else
                {
                    Debug.Log("Cannot place object. Placement point is already occupied.");
                }
            }
            else if (placedObject != null)
            {
                playerInteraction.PickUpObject(placedObject);
                placedObject = null;
                Debug.Log("Picked up object from interactable.");
            }
            else
            {
                Debug.Log("Nothing to interact with.");
            }
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                return placedObject == null;
            }
            else
            {
                return placedObject != null;
            }
        }
        return false;
    }
}
