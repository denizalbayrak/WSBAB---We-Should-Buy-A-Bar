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
                // Oyuncu bir nesne taþýyor
                if (placedObject != null)
                {
                    // Kabinde bir nesne var, etkileþimi kabindeki nesneye yönlendir
                    IInteractableItem carriedItem = carriedObject.GetComponent<IInteractableItem>();

                    if (carriedItem != null)
                    {
                        carriedItem.InteractWith(placedObject, this);

                        // Eðer taþýdýðý nesne etkileþim sonrasý yok edildiyse, carriedObject'i sýfýrla
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
                    // Kabin boþ, nesneyi kabine yerleþtir
                    base.Interact(player);
                    Debug.Log("Placed object on cabinet.");
                }
            }
            else
            {
                // Oyuncu bir þey taþýmýyor
                if (placedObject != null)
                {
                    // Nesneyi al
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
            return true; // Etkileþime her zaman izin veriyoruz
        }
    }


