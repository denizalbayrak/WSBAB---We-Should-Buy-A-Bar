using UnityEngine;
using System.Collections.Generic;

public class DirtyPoint : PlacableInteractable
{
    public int maxCapacity = 2; 
    public List<GameObject> placedObjects = new List<GameObject>(); 
    public List<Transform> slotTransforms; 
    public GlassType allowedGlassTypes;
    public void AddPlacedObject(GameObject obj)
    {
        if (placedObjects.Count >= maxCapacity)
        {
            Debug.Log("DirtyPoint is full.");
            return;
        }

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

        placedObjects.Add(obj);
        obj.transform.SetParent(availableSlot);

        obj.transform.position = availableSlot.position;
        obj.transform.rotation = availableSlot.rotation;
        obj.transform.localScale = Vector3.one; 
        GlassType carriedGlassType = GetGlassType(obj);
        if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Beer)
        {
            obj.GetComponent<BeerGlass>().Dirty();
        } 
        if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Wine)
        {
            obj.GetComponent<WineGlass>().Dirty();
        }
        if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Mojito)
        {
            obj.GetComponent<MojitoGlass>().Dirty();
        }
        if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Mimosa)
        {
            obj.GetComponent<MimosaGlass>().Dirty();
        } 
        if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Whiskey)
        {
            obj.GetComponent<WhiskeyGlass>().Dirty();
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
                    GameObject objToPickUp = placedObjects[0];
                    placedObjects.RemoveAt(0);

                    objToPickUp.transform.SetParent(null);
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
                    BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                    if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.DirtyEmpty)
                    {
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
                    WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
                    if (wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.DirtyEmpty)
                    {
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
                if (allowedGlassTypes == carriedGlassType && allowedGlassTypes == GlassType.Mojito)
                {
                    MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                    if (mojitoGlass != null && mojitoGlass.CurrentState == MojitoGlass.GlassState.DirtyEmpty)
                    {
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
                if (allowedGlassTypes == carriedGlassType && allowedGlassTypes == GlassType.Mimosa)
                {
                    MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                    if (mimosaGlass != null && mimosaGlass.CurrentState == MimosaGlass.GlassState.DirtyEmpty)
                    {
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
               if (allowedGlassTypes == carriedGlassType && allowedGlassTypes == GlassType.Whiskey)
                {
                    WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>();
                    if (whiskeyGlass != null && whiskeyGlass.CurrentState == WhiskeyGlass.GlassState.DirtyEmpty)
                    {
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
                    MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                    if (mimosaGlass != null && mimosaGlass.CurrentState == MimosaGlass.GlassState.DirtyEmpty)
                    {
                        foreach (Transform slot in slotTransforms)
                        {
                            if (slot.childCount == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                if (allowedGlassTypes == carriedGlassType && carriedGlassType == GlassType.Whiskey)
                {
                    WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>();
                    if (whiskeyGlass != null && whiskeyGlass.CurrentState == WhiskeyGlass.GlassState.DirtyEmpty)
                    {
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
        else if (obj.TryGetComponent<MimosaGlass>(out var mimosaGlass))
        {
            return mimosaGlass.glassType;
        }
        else if (obj.TryGetComponent<WhiskeyGlass>(out var whiskeyGlass))
        {
            return whiskeyGlass.glassType;
        }       

        return GlassType.Beer; 
    }
}
