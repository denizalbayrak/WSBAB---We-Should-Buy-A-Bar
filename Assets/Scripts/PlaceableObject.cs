using UnityEngine;

public enum PlacementType
{
    TableArea,
    CounterArea,
    ShelfArea
    // Diðer alan tipleri
}

public class PlaceableObject : MonoBehaviour
{
    public PlacementType placementType;
    private bool isCarried = false;

    // Taþýma ve yerleþtirme metodlarý
}

public class PlacementArea : MonoBehaviour
{
    public PlacementType areaType;

    // Yerleþtirme kontrol metodlarý
}
