using UnityEngine;
using System.Collections;

public class DeliveryPoint : PlacableInteractable
{
    public float dirtyGlassSpawnDelay = 5f; // Teslimattan sonra kirli bardaðýn spawn olma süresi
    public DirtyPoint dirtyPoint; // Kirli bardaklarýn spawn olacaðý nokta
    public GameObject beerGlass; // BeerGlass prefabý

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Player'ýn taþýdýðý objeyi bir deðiþkende saklayýn
                GameObject deliveredObject = playerInteraction.CarriedObject;

                // Player is carrying an object: Place it on the delivery point
                if (placedObject == null)
                {
                    base.Interact(player); // Objeyi yerleþtirir, CarriedObject null olur
                    Destroy(placedObject); // Teslimat sonrasý objeyi yok eder
                    Debug.Log("Delivered object at DeliveryPoint.");

                    // Sipariþi iþle
                    OrderManager.Instance.ProcessDeliveredItem(deliveredObject);

                    // Kirli bardaðýn spawn olmasýný zamanla
                    StartCoroutine(SpawnDirtyGlassAfterDelay());
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
    private IEnumerator SpawnDirtyGlassAfterDelay()
    {
        yield return new WaitForSeconds(dirtyGlassSpawnDelay);

        if (dirtyPoint != null)
        {
            // Kirli bardaðý oluþtur ve DirtyPoint'in placedObject'ine yerleþtir
            GameObject dirtyGlassObj = Instantiate(beerGlass, dirtyPoint.transform.position, Quaternion.identity, dirtyPoint.transform);
            BeerGlass dirtyGlass = dirtyGlassObj.GetComponent<BeerGlass>();
            if (dirtyGlass != null)
            {
                dirtyGlass.Dirty(); // Bardak durumunu kirli olarak ayarla
            }

            // DirtyPoint'in placedObject'ini ayarla
            dirtyPoint.SetPlacedObject(dirtyGlassObj);
        }
        else
        {
            Debug.LogError("DirtyPoint is not assigned in DeliveryPoint.");
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
