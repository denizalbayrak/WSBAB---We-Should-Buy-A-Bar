using UnityEngine;

[System.Serializable]
public class Order
{
    public int orderID; // Benzersiz sipariþ kimliði
    public OrderType orderType;
    public string description;
    public Sprite orderImage;
    public float timeLimit; // Sipariþin tamamlanmasý için verilen süre (saniye)
}


public enum OrderType
{
    DeliverBeerGlass,
    DeliverWineGlass,
    DeliverWaterGlass,
    // Diðer sipariþ türlerini ekleyin
}