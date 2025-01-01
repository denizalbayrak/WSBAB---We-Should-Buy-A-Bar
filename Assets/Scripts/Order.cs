using UnityEngine;

[System.Serializable]
public class Order
{
    public int orderID; 
    public OrderType orderType;
    public string description;
    public Sprite orderImage;
    public float timeLimit; 
    public float quickTimeLimit = 10f; 
    public int scorePerSuccess = 10; 
    public int scorePerSuccessQuick = 15; 
    public int scorePerFailure = -5; 
}

public enum OrderType
{
    DeliverBeerGlass,
    DeliverWineGlass,
    DeliverMojitoGlass,
    DeliverMimosaGlass,
    DeliverWhiskeyGlass
}
