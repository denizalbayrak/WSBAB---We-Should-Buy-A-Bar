using UnityEngine;

public class Counter : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    private CarryableObject placedObject;

    public bool PlaceObject(CarryableObject obj)
    {
        if (IsOccupied)
            return false;

        placedObject = obj;
        IsOccupied = true;
        return true;
    }

    public bool CanInteractWith(CarryableObject obj)
    {
        // Nesnenin bu tezgahla etkile�ime girip giremeyece�ini belirleyin
        // �rne�in, nesnenin t�r�ne veya �zelliklerine g�re kontrol yapabilirsiniz
        return true; // �imdilik her nesneyle etkile�ime girebilir olarak ayarlad�k
    }
}
