using UnityEngine;
using System.Collections.Generic;

public class DirtyPoint : PlacableInteractable
{
    public int maxCapacity = 2; // DirtyPoint'in maksimum kapasitesi
    public List<GameObject> placedObjects = new List<GameObject>(); // �u anki bardaklar
    public List<Transform> slotTransforms; // Bardaklar�n yerle�ece�i slotlar

    public void AddPlacedObject(GameObject obj)
    {
        // Kapasite kontrol�
        if (placedObjects.Count >= maxCapacity)
        {
            Debug.Log("DirtyPoint is full.");
            return;
        }

        // Bo� bir slot bul
        Transform availableSlot = null;
        foreach (Transform slot in slotTransforms)
        {
            if (slot.childCount == 0)
            {
                availableSlot = slot;
                break;
            }
        }

        if (availableSlot == null)
        {
            Debug.Log("No available slot to place the object.");
            return;
        }

        // Obje yerle�tir
        placedObjects.Add(obj);
        obj.transform.SetParent(availableSlot);

        obj.transform.position = availableSlot.position;
        obj.transform.rotation = availableSlot.rotation;
        obj.transform.localScale = Vector3.one; // �l�e�i ayarlay�n
        obj.GetComponent<BeerGlass>().Dirty();
        obj.SetActive(true);

        Debug.Log("Placed dirty glass on DirtyPoint.");
    }

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject == null)
            {
                if (placedObjects.Count > 0)
                {
                    // Oyuncu bo� ve DirtyPoint'te bardak var, bir barda�� al
                    GameObject objToPickUp = placedObjects[0];
                    placedObjects.RemoveAt(0);

                    // Barda�� slotundan kald�r
                    objToPickUp.transform.SetParent(null);
                    // Barda�� oyuncuya ver
                    playerInteraction.PickUpObject(objToPickUp);


                    Debug.Log("Picked up dirty glass from DirtyPoint.");
                }
                else
                {
                    Debug.Log("There is nothing to pick up on the DirtyPoint.");
                }
            }
            else
            {
                // Oyuncu bir �ey ta��yor, pis bardak yerle�tirmeye �al��
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.DirtyEmpty)
                {
                    // Pis barda�� DirtyPoint'e yerle�tir
                    AddPlacedObject(playerInteraction.CarriedObject);
                    playerInteraction.CarriedObject = null;
                    playerInteraction.isCarrying = false; // Merkezi y�netim i�in
                    playerInteraction.animator.SetBool("isCarry", false);
                }
                else
                {
                    Debug.Log("Cannot place object. Only dirty glasses can be placed on DirtyPoint.");
                }
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
                // Oyuncu bir �ey ta��m�yor, DirtyPoint'te bardak varsa alabilir
                return placedObjects.Count > 0;
            }
            else
            {
                // Oyuncu bir �ey ta��yor, pis bardak yerle�tirebilir mi kontrol et
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.DirtyEmpty)
                {
                    // Bo� bir slot var m� kontrol et
                    foreach (Transform slot in slotTransforms)
                    {
                        if (slot.childCount == 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        return false;
    }
}
