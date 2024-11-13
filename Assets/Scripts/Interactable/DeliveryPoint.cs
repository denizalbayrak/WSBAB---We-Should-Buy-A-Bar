using UnityEngine;

public class DeliveryPoint : PlacableInteractable
{
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Player'ýn taþýdýðý objeyi bir deðiþkende saklayýn
                GameObject deliveredObject = playerInteraction.CarriedObject;

                // Player is carrying an object: Place it on the delivery point
                if (placedObject == null)
                {
                    base.Interact(player); // Objeyi yerleþtirir, CarriedObject null olur
                    Destroy(placedObject); // Teslimat sonrasý objeyi yok eder
                    Debug.Log("Delivered object at DeliveryPoint.");

                    // Process the order using the saklanan objeyi
                    Order completedOrder = OrderManager.Instance.FindMatchingOrder(deliveredObject);
                    if (completedOrder != null)
                    {
                        OrderManager.Instance.ProcessOrder(completedOrder, true); // Baþarýlý teslimat
                    }
                    else
                    {
                        Debug.LogWarning("No matching order found for the delivered object.");
                    }
                }
                else
                {
                    Debug.Log("Cannot place object. DeliveryPoint is already occupied.");
                }
            }
            else
            {
                Debug.Log("Cannot deliver. No object is being carried.");
            }
        }
        else
        {
            Debug.Log("Cannot interact with the DeliveryPoint.");
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Can deliver if DeliveryPoint is empty
                return placedObject == null;
            }
            else
            {
                // Can interact only if carrying something to deliver
                return false;
            }
        }
        return false;
    }
}
