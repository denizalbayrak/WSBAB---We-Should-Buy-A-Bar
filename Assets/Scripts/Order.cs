using UnityEngine;

[System.Serializable]
public class Order
{
    public OrderType orderType; // Sipariþ türü
    public string description; // UI'da gösterilecek açýklama
    public Sprite orderImage; // UI'da gösterilecek resim
    public float timeLimit; // Sipariþin tamamlanmasý için verilen süre (saniye)
    public float delayAfterOrder; // Sipariþ tamamlandýktan sonra bekleme süresi (saniye)
}
