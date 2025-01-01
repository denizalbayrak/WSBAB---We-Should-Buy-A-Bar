using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Image orderImage;
    public TextMeshProUGUI orderNameText;
    public Image timerImage;
    public Slider timeSlider; 

    private Order currentOrder;
    private OrderManager orderManager;

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
            timerImage.fillAmount = 1f; 
            timerImage.color = Color.green; 
        }
    }

    public void UpdateTimer(float fillAmount)
    {
        if (timerImage != null)
        {
            timerImage.fillAmount = fillAmount;
            timeSlider.value = fillAmount;
            timerImage.color = Color.Lerp(Color.red, Color.green, fillAmount);
        }
    }

    public void RemoveUI()
    {
        Destroy(gameObject);
    }
}
