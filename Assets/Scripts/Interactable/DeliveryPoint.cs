using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeliveryPoint : PlacableInteractable
{
    public float dirtyGlassSpawnDelay = 5f; // Teslimattan sonra kirli bardaðýn spawn olma süresi
    public GameObject prefabToSpawn;
    // Bira ve þarap için ayrý DirtyPoint'ler
    public DirtyPoint beerDirtyPoint;
    public DirtyPoint wineDirtyPoint;
    public DirtyPoint mojitoDirtyPoint;

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

                // Player'ýn taþýdýðý objeyi bir deðiþkende saklayýn
                GameObject deliveredObject = playerInteraction.CarriedObject;

                // Teslimat iþlemi
                if (placedObject == null)
                {
                    base.Interact(player); // Objeyi yerleþtirir, CarriedObject null olur
                                           //   Destroy(placedObject); // Teslimat sonrasý objeyi yok eder
                    placedObject.SetActive(false); // Teslimat sonrasý objeyi yok eder
                    Debug.Log("Delivered object at DeliveryPoint.");

                    // Sipariþi iþle
                    OrderManager.Instance.ProcessDeliveredItem(deliveredObject);
                    StartCoroutine(SpawnDirtyGlassAfterDelay(deliveredObject));

                    // Kirli bardaðýn spawn olmasýný zamanla
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
        // Teslim edilen objenin prefab'ýný saklamak için referansý alýn
         prefabToSpawn = deliveredObject;

        // Nesneyi hemen yok etmiyoruz, Coroutine çalýþmaya devam ediyor
        yield return new WaitForSeconds(dirtyGlassSpawnDelay);

        // Prefab referansý üzerinden yeni nesne oluþtur
        if (prefabToSpawn != null)
        {
            // DirtyPoint için uygun hedefi belirleme (örneðin bira için farklý, þarap için farklý DirtyPoint)
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

            if (targetDirtyPoint != null)
            {
                // DirtyPoint'in kapasitesini kontrol et
                if (targetDirtyPoint.placedObjects.Count >= targetDirtyPoint.maxCapacity)
                {
                    Debug.Log("DirtyPoint is full. Cannot add more dirty glasses.");
                    yield break;
                }

                // Yeni bir nesne oluþtur ve DirtyPoint'e yerleþtir
                GameObject dirtyGlassObj = Instantiate(prefabToSpawn, targetDirtyPoint.transform.position, Quaternion.identity, targetDirtyPoint.transform);
                dirtyGlassObj.name = prefabToSpawn.name; // Ýsimlendirmeyi koru
                Destroy(prefabToSpawn);   
                // Bardak durumunu kirli olarak ayarla
                Carryable dirtyGlass = dirtyGlassObj.GetComponent<Carryable>();
                if (dirtyGlass != null)
                {
                    dirtyGlass.isReady = false;
                }

                // DirtyPoint'e bardaðý ekle
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
