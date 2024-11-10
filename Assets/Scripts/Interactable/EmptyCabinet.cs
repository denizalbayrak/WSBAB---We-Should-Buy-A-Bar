using UnityEngine;

public class EmptyCabinet : PlacableInteractable
{
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Player is carrying an object: Place it on the cabinet
                if (placedObject == null)
                {
                    GameObject carriedObject = playerInteraction.CarriedObject;

                    // Place the carried object on the cabinet
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

                    Debug.Log("Placed object on cabinet.");
                }
                else
                {
                    Debug.Log("Cannot place object on cabinet. It's already occupied.");
                }
            }
            else
            {
                // Player is not carrying anything: Pick up the object from the cabinet
                if (placedObject != null)
                {
                    // Pick up the object
                    playerInteraction.PickUpObject(placedObject);

                    // Remove the object from the cabinet
                    placedObject = null;

                    Debug.Log("Picked up object from cabinet.");
                }
                else
                {
                    Debug.Log("There is nothing to pick up on the cabinet.");
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
            if (playerInteraction.CarriedObject != null)
            {
                // Can interact if carrying an object and the cabinet is empty
                return placedObject == null;
            }
            else
            {
                // Can interact if not carrying anything and the cabinet has an object
                return placedObject != null;
            }
        }
        return false;
    }
}
