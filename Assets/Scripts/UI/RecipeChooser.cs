using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeChooser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Toggle recipeToggle;
    public Image recipeImage; 

    public Color selectedColor = Color.green;
    public Color hoverColor = Color.yellow;  
    private Color originalColor;              

    private void Start()
    {
        if (recipeImage != null)
        {
            originalColor = recipeImage.color;
        }

        if (recipeToggle != null)
        {
            recipeToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnDestroy()
    {
        if (recipeToggle != null)
        {
            recipeToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (recipeImage != null)
        {
            recipeImage.color = isOn ? selectedColor : originalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (recipeImage != null && !recipeToggle.isOn) 
        {
            recipeImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (recipeImage != null && !recipeToggle.isOn)
        {
            recipeImage.color = originalColor;
        }
    }
}
