using UnityEngine;

[System.Serializable]
public class Order
{
    public int orderID; // Benzersiz sipariþ kimliði
    public OrderType orderType;
    public string description;
    public Sprite orderImage;
    public float timeLimit; // Sipariþin tamamlanmasý için verilen süre (saniye)
    public int scorePerSuccess = 10; // Sipariþ baþarýyla tamamlandýðýnda kazanýlan puan
    public int scorePerFailure = -5; // Sipariþ baþarýsýz olduðunda kaybedilen puan
}



public enum OrderType
{
    DeliverBeerGlass,
    DeliverWineGlass,
    DeliverMojitoGlass,
    DeliverMimosaGlass,
    DeliverWhiskeyGlass
}