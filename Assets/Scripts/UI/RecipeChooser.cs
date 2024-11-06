using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeChooser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Toggle recipeToggle;
    public Image recipeImage; // Resmin rengini de�i�tirmek i�in

    public Color selectedColor = Color.green; // Toggle se�ildi�inde uygulanacak renk
    public Color hoverColor = Color.yellow;   // Hover durumunda uygulanacak renk
    private Color originalColor;              // Ba�lang�� rengi

    private void Start()
    {
        // Orijinal rengi sakla
        if (recipeImage != null)
        {
            originalColor = recipeImage.color;
        }

        // Toggle i�in event dinleyici ekle
        if (recipeToggle != null)
        {
            recipeToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnDestroy()
    {
        // Dinleyiciyi kald�r (�nlem olarak)
        if (recipeToggle != null)
        {
            recipeToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }

    // Toggle durumu de�i�ti�inde �a�r�lan metot
    private void OnToggleValueChanged(bool isOn)
    {
        if (recipeImage != null)
        {
            recipeImage.color = isOn ? selectedColor : originalColor;
        }
    }

    // Fareyle �zerine gelindi�inde (hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (recipeImage != null && !recipeToggle.isOn) // Sadece toggle se�ili de�ilse hover efekti uygula
        {
            recipeImage.color = hoverColor;
        }
    }

    // Fareyle �zerinden ��k�ld���nda
    public void OnPointerExit(PointerEventData eventData)
    {
        if (recipeImage != null && !recipeToggle.isOn) // Sadece toggle se�ili de�ilse orijinal renge d�n
        {
            recipeImage.color = originalColor;
        }
    }
}
