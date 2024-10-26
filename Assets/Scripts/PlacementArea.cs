using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    public PlacementType areaType;
    public bool isOccupied = false;

    void OnTriggerEnter(Collider other)
    {
        PlaceableObject obj = other.GetComponent<PlaceableObject>();
        if (obj != null && obj.placementType == areaType && !isOccupied)
        {
            // Obje bu alana yerleþtirilebilir
            // Görsel olarak vurgula
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Vurgulamayý kaldýr
    }
}
