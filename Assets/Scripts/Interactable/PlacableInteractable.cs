using UnityEngine;

public class PlacableInteractable : Interactable
{
    public Transform placementPoint; // The point where objects can be placed
    protected GameObject placedObject; // The object currently placed on the interactable

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                if (placedObject == null)
                {
                    // Place the carried object on the interactable
                    GameObject carriedObject = playerInteraction.CarriedObject;

                    // Set the parent to the placement point
                    carriedObject.transform.SetParent(placementPoint);

                    // Reset local position and rotation to align with the placement point
                    carriedObject.transform.localPosition = Vector3.zero;
                    carriedObject.transform.localRotation = Quaternion.identity;

                    // Notify the carried object
                    Carryable carryable = carriedObject.GetComponent<Carryable>();
                    if (carryable != null)
                    {
                        carryable.OnDrop();
                    }

                    // Reset the player's carried object
                    playerInteraction.CarriedObject = null;

                    // Keep track of the placed object
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
                // Pick up the placed object
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
                // Can place an object if placement point is empty
                return placedObject == null;
            }
            else
            {
                // Can pick up the placed object if one exists
                return placedObject != null;
            }
        }
        return false;
    }
}
