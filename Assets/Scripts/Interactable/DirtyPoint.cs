using UnityEngine;
using System.Collections.Generic;

public class DirtyPoint : PlacableInteractable
{
    public int maxCapacity = 2; // DirtyPoint'in maksimum kapasitesi
    public List<GameObject> placedObjects = new List<GameObject>(); // Þu anki bardaklar

    public List<Transform> slotTransforms; // Bardaklarýn yerleþeceði slotlar

    public void AddPlacedObject(GameObject obj)
    {
        if (placedObjects.Count >= maxCapacity)
        {
            Debug.Log("DirtyPoint is full.");
            return;
        }

        placedObjects.Add(obj);
        obj.transform.SetParent(placementPoint);

        // Bardaklarý slotlara yerleþtir
        Transform slotTransform = slotTransforms[placedObjects.Count - 1];
        obj.transform.position = slotTransform.position;
        obj.transform.rotation = slotTransform.rotation;
        obj.transform.localScale = Vector3.one; // Ölçeði ayarlayýn

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
                    // Oyuncu boþ ve DirtyPoint'te bardak var, bir bardaðý al
                    GameObject objToPickUp = placedObjects[0];
                    placedObjects.RemoveAt(0);

                    // Bardaðý oyuncuya ver
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
                // Oyuncu bir þey taþýyor, DirtyPoint'e obje yerleþtiremez
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
                // Oyuncu bir þey taþýmýyor, DirtyPoint'te bardak varsa alabilir
                return placedObjects.Count > 0;
            }
            else
            {
                // Oyuncu bir þey taþýyor, DirtyPoint'e obje yerleþtiremez
                return false;
            }
        }
        return false;
    }
}
