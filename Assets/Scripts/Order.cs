using UnityEngine;

[System.Serializable]
public class Order
{
    public int orderID; // Benzersiz sipari� kimli�i
    public OrderType orderType;
    public string description;
    public Sprite orderImage;
    public float timeLimit; // Sipari�in tamamlanmas� i�in verilen s�re (saniye)
    public int scorePerSuccess = 10; // Sipari� ba�ar�yla tamamland���nda kazan�lan puan
    public int scorePerFailure = -5; // Sipari� ba�ar�s�z oldu�unda kaybedilen puan
}



public enum OrderType
{
    DeliverBeerGlass,
    DeliverWineGlass,
    DeliverMojitoGlass,
    DeliverMimosaGlass,
    DeliverWhiskeyGlass
}