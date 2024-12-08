using UnityEngine;
using System.Collections.Generic;

public class DirtyPoint : PlacableInteractable
{
    public int maxCapacity = 2; // DirtyPoint'in maksimum kapasitesi
    public List<GameObject> placedObjects = new List<GameObject>(); // Þu anki bardaklar
    public List<Transform> slotTransforms; // Bardaklarýn yerleþeceði slotlar
    public GlassType allowedGlassTypes;
    public void AddPlacedObject(GameObject obj)
    {
        // Kapasite kontrolü
        if (placedObjects.Count >= maxCapacity)
        {
            Debug.Log("DirtyPoint is full.");
            return;
        }

        // Boþ bir slot bul
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

        // Obje yerleþtir
        placedObjects.Add(obj);
        obj.transform.SetParent(availableSlot);

        obj.transform.position = availableSlot.position;
        obj.transform.rotation = availableSlot.rotation;
        obj.transform.localScale = Vector3.one; // Ölçeði ayarlayýn
        GlassType carriedGlassType = GetGlassType(obj);
        if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Beer)
        {
            obj.GetComponent<BeerGlass>().Dirty();
        } 
        if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Wine)
        {
            obj.GetComponent<WineGlass>().Dirty();
        }
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
                    // Oyuncu boþ ve DirtyPoint'te bardak var, bir bardaðý al
                    GameObject objToPickUp = placedObjects[0];
                    placedObjects.RemoveAt(0);

                    // Bardaðý slotundan kaldýr
                    objToPickUp.transform.SetParent(null);
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
                GlassType carriedGlassType = GetGlassType(playerInteraction.CarriedObject);
                if (allowedGlassTypes == carriedGlassType && allowedGlassTypes == GlassType.Beer)
                {
                    // Oyuncu bir þey taþýyor, pis bardak yerleþtirmeye çalýþ
                    BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                    if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.DirtyEmpty)
                    {
                        // Pis bardaðý DirtyPoint'e yerleþtir
                        AddPlacedObject(playerInteraction.CarriedObject);
                        playerInteraction.CarriedObject = null;
                        playerInteraction.isCarrying = false;
                        playerInteraction.animator.SetBool("isCarry", false);
                    }
                    else
                    {
                        Debug.Log("Cannot place object. Only dirty glasses can be placed on DirtyPoint.");
                    }
                }
                if (allowedGlassTypes == carriedGlassType && allowedGlassTypes == GlassType.Wine)
                {
                    // Oyuncu bir þey taþýyor, pis bardak yerleþtirmeye çalýþ
                    WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
                    if (wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.DirtyEmpty)
                    {
                        // Pis bardaðý DirtyPoint'e yerleþtir
                        AddPlacedObject(playerInteraction.CarriedObject);
                        playerInteraction.CarriedObject = null;
                        playerInteraction.isCarrying = false; // Merkezi yönetim için
                        playerInteraction.animator.SetBool("isCarry", false);
                    }
                    else
                    {
                        Debug.Log("Cannot place object. Only dirty glasses can be placed on DirtyPoint.");
                    }
                }
                if (allowedGlassTypes == carriedGlassType && allowedGlassTypes == GlassType.Mojito)
                {
                    // Oyuncu bir þey taþýyor, pis bardak yerleþtirmeye çalýþ
                    MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                    if (mojitoGlass != null && mojitoGlass.CurrentState == MojitoGlass.GlassState.DirtyEmpty)
                    {
                        // Pis bardaðý DirtyPoint'e yerleþtir
                        AddPlacedObject(playerInteraction.CarriedObject);
                        playerInteraction.CarriedObject = null;
                        playerInteraction.isCarrying = false; // Merkezi yönetim için
                        playerInteraction.animator.SetBool("isCarry", false);
                    }
                    else
                    {
                        Debug.Log("Cannot place object. Only dirty glasses can be placed on DirtyPoint.");
                    }
                } 
                if (allowedGlassTypes == carriedGlassType && allowedGlassTypes == GlassType.Mimosa)
                {
                    // Oyuncu bir þey taþýyor, pis bardak yerleþtirmeye çalýþ
                    MojitoGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                    if (mimosaGlass != null && mimosaGlass.CurrentState == MojitoGlass.GlassState.DirtyEmpty)
                    {
                        // Pis bardaðý DirtyPoint'e yerleþtir
                        AddPlacedObject(playerInteraction.CarriedObject);
                        playerInteraction.CarriedObject = null;
                        playerInteraction.isCarrying = false; // Merkezi yönetim için
                        playerInteraction.animator.SetBool("isCarry", false);
                    }
                    else
                    {
                        Debug.Log("Cannot place object. Only dirty glasses can be placed on DirtyPoint.");
                    }
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
                // Oyuncu bir þey taþýmýyor, DirtyPoint'te bardak varsa alabilir
                return placedObjects.Count > 0;
            }
            else
            {
                GlassType carriedGlassType = GetGlassType(playerInteraction.CarriedObject);
                if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Beer)
                {
                    BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                    if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.DirtyEmpty)
                    {
                        // Boþ bir slot var mý kontrol et
                        foreach (Transform slot in slotTransforms)
                        {
                            if (slot.childCount == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Wine)
                {
                    WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
                    if (wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.DirtyEmpty)
                    {
                        // Boþ bir slot var mý kontrol et
                        foreach (Transform slot in slotTransforms)
                        {
                            if (slot.childCount == 0)
                            {
                                return true;
                            }
                        }
                    }
                } 
                if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Mojito)
                {
                    MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                    if (mojitoGlass != null && mojitoGlass.CurrentState == MojitoGlass.GlassState.DirtyEmpty)
                    {
                        // Boþ bir slot var mý kontrol et
                        foreach (Transform slot in slotTransforms)
                        {
                            if (slot.childCount == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Mimosa)
                {
                    MojitoGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                    if (mimosaGlass != null && mimosaGlass.CurrentState == MojitoGlass.GlassState.DirtyEmpty)
                    {
                        // Boþ bir slot var mý kontrol et
                        foreach (Transform slot in slotTransforms)
                        {
                            if (slot.childCount == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }
        return false;
    }
    private GlassType GetGlassType(GameObject obj)
    {
        if (obj.TryGetComponent<BeerGlass>(out var beerGlass))
        {
            return beerGlass.glassType;
        }
        else if (obj.TryGetComponent<WineGlass>(out var wineGlass))
        {
            return wineGlass.glassType;
        } 
        else if (obj.TryGetComponent<MojitoGlass>(out var mojitoGlass))
        {
            return mojitoGlass.glassType;
        } 
        else if (obj.TryGetComponent<MojitoGlass>(out var mimosaGlass))
        {
            return mimosaGlass.glassType;
        }
        //else if (obj.TryGetComponent<WhiskeyGlass>(out var whiskeyGlass))
        //{
        //    return whiskeyGlass.glassType;
        //}
        // Diðer bardak tipleri için ekleyebilirsiniz

        return GlassType.Beer; // Varsayýlan deðer, isterseniz deðiþtirebilirsiniz
    }
}
