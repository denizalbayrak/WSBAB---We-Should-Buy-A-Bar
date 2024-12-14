using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Image orderImage;
    public TextMeshProUGUI orderNameText;
    public Image timerImage; // Renk de�i�imi i�in
    public Slider timeSlider; // Renk de�i�imi i�in

    private Order currentOrder;
    private OrderManager orderManager;

    /// <summary>
    /// Sipari�i ayarlar ve UI'ya yans�t�r.
    /// </summary>
    /// <param name="order">Olu�turulan sipari�.</param>
    /// <param name="manager">OrderManager referans�.</param>
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
            timerImage.fillAmount = 1f; // Ba�lang��ta tam dolu
            timerImage.color = Color.green; // Ba�lang�� rengi ye�il
        }
    }

    /// <summary>
    /// Timer'� g�nceller ve renk de�i�imini kontrol eder.
    /// </summary>
    /// <param name="fillAmount">Timer'�n doluluk oran� (0-1).</param>
    public void UpdateTimer(float fillAmount)
    {
        if (timerImage != null)
        {
            timerImage.fillAmount = fillAmount;
            timeSlider.value = fillAmount;
            // Renk ge�i�i: Ye�ilden K�rm�z�ya
            timerImage.color = Color.Lerp(Color.red, Color.green, fillAmount);
        }
    }

    /// <summary>
    /// UI eleman�n� kald�r�r.
    /// </summary>
    public void RemoveUI()
    {
        Destroy(gameObject);
    }
}
