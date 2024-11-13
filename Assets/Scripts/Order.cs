using UnityEngine;

[System.Serializable]
public class Order
{
    public OrderType orderType; // Sipari� t�r�
    public string description; // UI'da g�sterilecek a��klama
    public Sprite orderImage; // UI'da g�sterilecek resim
    public float timeLimit; // Sipari�in tamamlanmas� i�in verilen s�re (saniye)
    public float delayAfterOrder; // Sipari� tamamland�ktan sonra bekleme s�resi (saniye)
}
