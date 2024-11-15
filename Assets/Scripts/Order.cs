using UnityEngine;

[System.Serializable]
public class Order
{
    public int orderID; // Benzersiz sipari� kimli�i
    public OrderType orderType;
    public string description;
    public Sprite orderImage;
    public float timeLimit; // Sipari�in tamamlanmas� i�in verilen s�re (saniye)
}


public enum OrderType
{
    DeliverBeerGlass,
    DeliverWineGlass,
    DeliverWaterGlass,
    // Di�er sipari� t�rlerini ekleyin
}