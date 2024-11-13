// Order.cs
using UnityEngine;

[System.Serializable]
public class Order
{
    public OrderType orderType; // Enum kullanýmý
    public string description; // UI'da gösterilecek açýklama
    public Sprite orderImage; // UI'da gösterilecek resim
    public float timeLimit; // Sipariþin tamamlanmasý için verilen süre (saniye)
}
