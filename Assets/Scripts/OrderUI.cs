using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Image orderImage;
    public TextMeshProUGUI orderNameText;
    public Image timerImage; // Renk deðiþimi için
    public Slider timeSlider; // Renk deðiþimi için

    private Order currentOrder;
    private OrderManager orderManager;

    /// <summary>
    /// Sipariþi ayarlar ve UI'ya yansýtýr.
    /// </summary>
    /// <param name="order">Oluþturulan sipariþ.</param>
    /// <param name="manager">OrderManager referansý.</param>
    public void Setup(Order order, OrderManager manager)
    {
        currentOrder = order;
        orderManager = manager;

        if (orderImage != null)
        {
            orderImage.sprite = order.orderImage;
        }

        if (orderNameText != null)
        {
            orderNameText.text = order.description.ToString();
        }

        if (timerImage != null)
        {
            timeSlider.value = 1;
            timerImage.fillAmount = 1f; // Baþlangýçta tam dolu
            timerImage.color = Color.green; // Baþlangýç rengi yeþil
        }
    }

    /// <summary>
    /// Timer'ý günceller ve renk deðiþimini kontrol eder.
    /// </summary>
    /// <param name="fillAmount">Timer'ýn doluluk oraný (0-1).</param>
    public void UpdateTimer(float fillAmount)
    {
        if (timerImage != null)
        {
            timerImage.fillAmount = fillAmount;
            timeSlider.value = fillAmount;
            // Renk geçiþi: Yeþilden Kýrmýzýya
            timerImage.color = Color.Lerp(Color.red, Color.green, fillAmount);
        }
    }

    /// <summary>
    /// UI elemanýný kaldýrýr.
    /// </summary>
    public void RemoveUI()
    {
        Destroy(gameObject);
    }
}
