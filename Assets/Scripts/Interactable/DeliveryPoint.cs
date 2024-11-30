using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeliveryPoint : PlacableInteractable
{
    public float dirtyGlassSpawnDelay = 5f; // Teslimattan sonra kirli barda��n spawn olma s�resi
    public GameObject prefabToSpawn;
    // Bira ve �arap i�in ayr� DirtyPoint'ler
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

                // Player'�n ta��d��� objeyi bir de�i�kende saklay�n
                GameObject deliveredObject = playerInteraction.CarriedObject;

                // Teslimat i�lemi
                if (placedObject == null)
                {
                    base.Interact(player); // Objeyi yerle�tirir, CarriedObject null olur
                                           //   Destroy(placedObject); // Teslimat sonras� objeyi yok eder
                    placedObject.SetActive(false); // Teslimat sonras� objeyi yok eder
                    Debug.Log("Delivered object at DeliveryPoint.");

                    // Sipari�i i�le
                    OrderManager.Instance.ProcessDeliveredItem(deliveredObject);
                    StartCoroutine(SpawnDirtyGlassAfterDelay(deliveredObject));

                    // Kirli barda��n spawn olmas�n� zamanla
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
        // Teslim edilen objenin prefab'�n� saklamak i�in referans� al�n
         prefabToSpawn = deliveredObject;

        // Nesneyi hemen yok etmiyoruz, Coroutine �al��maya devam ediyor
        yield return new WaitForSeconds(dirtyGlassSpawnDelay);

        // Prefab referans� �zerinden yeni nesne olu�tur
        if (prefabToSpawn != null)
        {
            // DirtyPoint i�in uygun hedefi belirleme (�rne�in bira i�in farkl�, �arap i�in farkl� DirtyPoint)
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

                // Yeni bir nesne olu�tur ve DirtyPoint'e yerle�tir
                GameObject dirtyGlassObj = Instantiate(prefabToSpawn, targetDirtyPoint.transform.position, Quaternion.identity, targetDirtyPoint.transform);
                dirtyGlassObj.name = prefabToSpawn.name; // �simlendirmeyi koru
                Destroy(prefabToSpawn);   
                // Bardak durumunu kirli olarak ayarla
                Carryable dirtyGlass = dirtyGlassObj.GetComponent<Carryable>();
                if (dirtyGlass != null)
                {
                    dirtyGlass.isReady = false;
                }

                // DirtyPoint'e barda�� ekle
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
