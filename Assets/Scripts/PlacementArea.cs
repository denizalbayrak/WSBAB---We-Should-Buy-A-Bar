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
            // Obje bu alana yerle�tirilebilir
            // G�rsel olarak vurgula
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Vurgulamay� kald�r
    }
}
