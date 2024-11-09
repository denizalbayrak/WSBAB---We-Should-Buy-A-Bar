using UnityEngine;

public class PlacableInteractable : Interactable
{
    public Transform placementPoint; // The point where objects can be placed

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null && playerInteraction.CarriedObject != null)
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

            Debug.Log("Placed object on interactable.");
        }
        else
        {
            Debug.Log("Cannot place object. Either not carrying anything or invalid object.");
        }
    }

    public override bool CanInteract(GameObject player)
    {
        // Can interact only if the player is carrying an object
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        return playerInteraction != null && playerInteraction.CarriedObject != null;
    }
}
