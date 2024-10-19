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
        // Nesnenin bu tezgahla etkileþime girip giremeyeceðini belirleyin
        // Örneðin, nesnenin türüne veya özelliklerine göre kontrol yapabilirsiniz
        return true; // Þimdilik her nesneyle etkileþime girebilir olarak ayarladýk
    }
}
