using UnityEngine;

public enum PlacementType
{
    TableArea,
    CounterArea,
    ShelfArea
    // Di�er alan tipleri
}

public class PlaceableObject : MonoBehaviour
{
    public PlacementType placementType;
    private bool isCarried = false;

    // Ta��ma ve yerle�tirme metodlar�
}

public class PlacementArea : MonoBehaviour
{
    public PlacementType areaType;

    // Yerle�tirme kontrol metodlar�
}
