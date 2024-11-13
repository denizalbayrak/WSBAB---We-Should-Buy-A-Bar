// Order.cs
using UnityEngine;

[System.Serializable]
public class Order
{
    public OrderType orderType; // Enum kullan�m�
    public string description; // UI'da g�sterilecek a��klama
    public Sprite orderImage; // UI'da g�sterilecek resim
    public float timeLimit; // Sipari�in tamamlanmas� i�in verilen s�re (saniye)
}
