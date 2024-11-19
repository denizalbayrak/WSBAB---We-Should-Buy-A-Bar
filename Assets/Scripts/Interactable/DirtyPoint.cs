using UnityEngine;
using System.Collections.Generic;

public class DirtyPoint : PlacableInteractable
{
    public int maxCapacity = 2; // DirtyPoint'in maksimum kapasitesi
    public List<GameObject> placedObjects = new List<GameObject>(); // �u anki bardaklar

    public List<Transform> slotTransforms; // Bardaklar�n yerle�ece�i slotlar

    public void AddPlacedObject(GameObject obj)
    {
        if (placedObjects.Count >= maxCapacity)
        {
            Debug.Log("DirtyPoint is full.");
            return;
        }

        placedObjects.Add(obj);
        obj.transform.SetParent(placementPoint);

        // Bardaklar� slotlara yerle�tir
        Transform slotTransform = slotTransforms[placedObjects.Count - 1];
        obj.transform.position = slotTransform.position;
        obj.transform.rotation = slotTransform.rotation;
        obj.transform.localScale = Vector3.one; // �l�e�i ayarlay�n

        obj.SetActive(true);
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
                // Oyuncu bir �ey ta��yor, DirtyPoint'e obje yerle�tiremez
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
                // Oyuncu bir �ey ta��m�yor, DirtyPoint'te bardak varsa alabilir
                return placedObjects.Count > 0;
            }
            else
            {
                // Oyuncu bir �ey ta��yor, DirtyPoint'e obje yerle�tiremez
                return false;
            }
        }
        return false;
    }
}
