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
                // Oyuncu bir nesne ta��yor
                if (placedObject != null)
                {
                    // Kabinde bir nesne var, etkile�imi kabindeki nesneye y�nlendir
                    IInteractableItem carriedItem = carriedObject.GetComponent<IInteractableItem>();

                    if (carriedItem != null)
                    {
                        carriedItem.InteractWith(placedObject, this);

                        // E�er ta��d��� nesne etkile�im sonras� yok edildiyse, carriedObject'i s�f�rla
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
                    // Kabin bo�, nesneyi kabine yerle�tir
                    base.Interact(player);
                    Debug.Log("Placed object on cabinet.");
                }
            }
            else
            {
                // Oyuncu bir �ey ta��m�yor
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
            return true; // Etkile�ime her zaman izin veriyoruz
        }
    }


