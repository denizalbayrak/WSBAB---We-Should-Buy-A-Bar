using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeChooser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Toggle recipeToggle;
    public Image recipeImage; // Resmin rengini deðiþtirmek için

    public Color selectedColor = Color.green; // Toggle seçildiðinde uygulanacak renk
    public Color hoverColor = Color.yellow;   // Hover durumunda uygulanacak renk
    private Color originalColor;              // Baþlangýç rengi

    private void Start()
    {
        // Orijinal rengi sakla
        if (recipeImage != null)
        {
            originalColor = recipeImage.color;
        }

        // Toggle için event dinleyici ekle
        if (recipeToggle != null)
        {
            recipeToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnDestroy()
    {
        // Dinleyiciyi kaldýr (önlem olarak)
        if (recipeToggle != null)
        {
            recipeToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }

    // Toggle durumu deðiþtiðinde çaðrýlan metot
    private void OnToggleValueChanged(bool isOn)
    {
        if (recipeImage != null)
        {
            recipeImage.color = isOn ? selectedColor : originalColor;
        }
    }

    // Fareyle üzerine gelindiðinde (hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (recipeImage != null && !recipeToggle.isOn) // Sadece toggle seçili deðilse hover efekti uygula
        {
            recipeImage.color = hoverColor;
        }
    }

    // Fareyle üzerinden çýkýldýðýnda
    public void OnPointerExit(PointerEventData eventData)
    {
        if (recipeImage != null && !recipeToggle.isOn) // Sadece toggle seçili deðilse orijinal renge dön
        {
            recipeImage.color = originalColor;
        }
    }
}
