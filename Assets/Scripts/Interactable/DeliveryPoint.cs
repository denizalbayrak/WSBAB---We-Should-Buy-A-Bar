using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeliveryPoint : PlacableInteractable
{
    public float dirtyGlassSpawnDelay = 5f; 
    public GameObject prefabToSpawn;
    public DirtyPoint beerDirtyPoint;
    public DirtyPoint wineDirtyPoint;
    public DirtyPoint mojitoDirtyPoint;
    public DirtyPoint mimosaDirtyPoint;
    public DirtyPoint whiskeyDirtyPoint;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                Carryable carriedItem = playerInteraction.CarriedObject.GetComponent<Carryable>();
                if (carriedItem != null && !carriedItem.isReady)
                {
                    return;
                }

                GameObject deliveredObject = playerInteraction.CarriedObject;

                if (placedObject == null)
                {
                    base.Interact(player);
                                           
                    placedObject.SetActive(false); 
                    Debug.Log("Delivered object at DeliveryPoint.");

                    OrderManager.Instance.ProcessDeliveredItem(deliveredObject);
                    StartCoroutine(SpawnDirtyGlassAfterDelay(deliveredObject));

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

    private IEnumerator SpawnDirtyGlassAfterDelay(GameObject deliveredObject)
    {
         prefabToSpawn = deliveredObject;

        yield return new WaitForSeconds(dirtyGlassSpawnDelay);

        if (prefabToSpawn != null)
        {
            DirtyPoint targetDirtyPoint = null;

            if (prefabToSpawn.name.Contains("Beer"))
            {
                targetDirtyPoint = beerDirtyPoint;
            }
            else if (prefabToSpawn.name.Contains("Wine"))
            {
                targetDirtyPoint = wineDirtyPoint;
            }
            else if (prefabToSpawn.name.Contains("Mojito"))
            {
                targetDirtyPoint = mojitoDirtyPoint;
            }
            else if (prefabToSpawn.name.Contains("Mimosa"))
            {
                targetDirtyPoint = mimosaDirtyPoint;
            } 
            else if (prefabToSpawn.name.Contains("Whiskey"))
            {
                targetDirtyPoint = whiskeyDirtyPoint;
            }

            if (targetDirtyPoint != null)
            {
                if (targetDirtyPoint.placedObjects.Count >= targetDirtyPoint.maxCapacity)
                {
                    Debug.Log("DirtyPoint is full. Cannot add more dirty glasses.");
                    yield break;
                }
                GameObject dirtyGlassObj = Instantiate(prefabToSpawn, targetDirtyPoint.transform.position, Quaternion.identity, targetDirtyPoint.transform);
                dirtyGlassObj.name = prefabToSpawn.name;
                Destroy(prefabToSpawn);   
                Carryable dirtyGlass = dirtyGlassObj.GetComponent<Carryable>();
                if (dirtyGlass != null)
                {
                    dirtyGlass.isReady = false;
                }
                targetDirtyPoint.AddPlacedObject(dirtyGlassObj);
            }
            else
            {
                Debug.LogError("No suitable DirtyPoint found for the delivered object.");
            }
        }
    }



    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                return placedObject == null;
            }
            else
            {
                
                return false;
            }
        }
        return false;
    }
}
