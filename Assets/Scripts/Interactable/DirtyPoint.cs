using UnityEngine;

public class DirtyPoint : PlacableInteractable
{
    // Yeni eklenen metod
    public void SetPlacedObject(GameObject obj)
    {
        placedObject = obj;

        placedObject.transform.SetParent(placementPoint);
        placedObject.transform.localPosition = Vector3.zero;
        placedObject.transform.localRotation = Quaternion.identity;
        placedObject.transform.localScale= new Vector3(77.56666f, 775.6667f, 77.56666f);
    }

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                // Player is not carrying anything: Pick up the object from the DirtyPoint
                if (placedObject != null)
                {
                    // Pick up the object
                    playerInteraction.PickUpObject(placedObject);

                    // Remove the object from the DirtyPoint
                    placedObject = null;

                    Debug.Log("Picked up dirty glass from DirtyPoint.");
                }
                else
                {
                    Debug.Log("There is nothing to pick up on the DirtyPoint.");
                }
            }
            else
            {
                // Player is carrying something: Cannot place objects on DirtyPoint
                Debug.Log("Cannot place objects on DirtyPoint.");
            }
        }
        else
        {
            Debug.Log("Cannot interact with the DirtyPoint.");
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                // Player is not carrying anything: Can pick up if there is a placed object
                return placedObject != null;
            }
            else
            {
                // Player is carrying something: Cannot place objects on DirtyPoint
                return false;
            }
        }
        return false;
    }
}
