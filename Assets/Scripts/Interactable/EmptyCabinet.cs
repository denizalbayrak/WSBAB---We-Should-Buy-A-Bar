using UnityEngine;

public class EmptyCabinet : PlacableInteractable
{
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null && playerInteraction.CarriedObject != null)
        {
            // Place the carried object on the cabinet
            GameObject carriedObject = playerInteraction.CarriedObject;

            // Detach the object from the player
            carriedObject.transform.SetParent(null);

            // Calculate the vertical offset based on the object's bounds
            Collider objectCollider = carriedObject.GetComponent<Collider>();
            float yOffset = 0f;
            if (objectCollider != null)
            {
                yOffset = objectCollider.bounds.extents.y;
            }

            // Place it at the placement point with the offset
            carriedObject.transform.position = placementPoint.position + new Vector3(0, yOffset, 0);
            carriedObject.transform.rotation = placementPoint.rotation;

            // Notify the carried object
            Carryable carryable = carriedObject.GetComponent<Carryable>();
            if (carryable != null)
            {
                carryable.OnDrop();
            }

            // Reset the player's carried object
            playerInteraction.CarriedObject = null;

            Debug.Log("Placed object on cabinet.");
        }
        else
        {
            Debug.Log("Cannot place object on cabinet. Either not carrying anything or invalid object.");
        }
    }

    public override bool CanInteract(GameObject player)
    {
        // Can interact only if the player is carrying an object
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        return playerInteraction != null && playerInteraction.CarriedObject != null;
    }
}
