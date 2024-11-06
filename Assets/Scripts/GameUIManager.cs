using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    public List<GameObject> recipes;
    public GameObject recipeTopUIPrefab;
    public GameObject recipeToggleUIPrefab;
    public GameObject MoreRecipeUI;
    public Transform ownedRecipesContainer; // Horizontal layout olan container
    public Transform chooseRecipeContainer; // Tüm tariflerin listelendiði panel

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadLevelRecipesUI()
    {
        // Kullanýcýnýn tariflerini SaveData'dan al
        List<string> ownedRecipes = GameManager.Instance.currentSaveData.ownedRecipeNames;
        // Mevcut tarif UI'lerini temizle
        foreach (Transform child in ownedRecipesContainer)
        {
            Destroy(child.gameObject);
        }
        int visibleRecipeCount = 0;
        foreach (var recipeName in ownedRecipes)
        {
            Recipe recipe = ItemHolder.Instance.GetRecipe(recipeName);
            if (recipe != null)
            {
                if (visibleRecipeCount < 3)
                {
                    GameObject newRecipeUI = Instantiate(recipeTopUIPrefab, ownedRecipesContainer);
                newRecipeUI.GetComponentInChildren<TextMeshProUGUI>().text = recipe.recipeName;
                newRecipeUI.transform.GetChild(0).GetComponent<Image>().sprite = recipe.recipeImage;
                }
                visibleRecipeCount++;
            }
            else
            {
                Debug.LogWarning("Recipe not found in ItemHolder: " + recipeName);
            }
        }
        LoadChooseRecipesUI();

    }
    public void LoadChooseRecipesUI()
    {
        List<string> ownedRecipes = GameManager.Instance.currentSaveData.ownedRecipeNames;

        // Mevcut tarif UI'lerini temizle
        foreach (Transform child in chooseRecipeContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (var recipeName in ownedRecipes)
        {
            Recipe recipe = ItemHolder.Instance.GetRecipe(recipeName);
            if (recipe != null)
            {
                // `chooseRecipeContainer` için toggle'ý oluþtur
                GameObject newRecipeToggleUI = Instantiate(recipeToggleUIPrefab, chooseRecipeContainer);
                newRecipeToggleUI.GetComponentInChildren<TextMeshProUGUI>().text = recipe.recipeName;
                newRecipeToggleUI.transform.GetChild(0).GetComponent<Image>().sprite = recipe.recipeImage;

                // Toggle bileþenini al ve ayarla
                Toggle toggle = newRecipeToggleUI.GetComponentInChildren<Toggle>();
                if (toggle != null)
                {
                    toggle.onValueChanged.AddListener(isOn => OnToggleValueChanged(isOn, recipe));

                    // Toggle'ýn baþlangýç durumunu ayarla
                    bool isSelected = recipe.recipeName == "Beer" || GameManager.Instance.currentSaveData.selectedRecipeNames.Contains(recipe.recipeName);
                    toggle.isOn = isSelected;

                    // Yalnýzca `Toggle` seçiliyse `ownedRecipesContainer`'a ekle
                    if (isSelected && !IsRecipeInOwnedContainer(recipe.recipeName))
                    {
                        AddRecipeToOwnedContainer(recipe);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Recipe not found in ItemHolder: " + recipeName);
            }
        }
    }

    private void AddRecipeToOwnedContainer(Recipe recipe)
    {
        // Zaten mevcutsa ekleme
        if (IsRecipeInOwnedContainer(recipe.recipeName))
        {
            return;
        }

        // Yeni bir UI oluþtur ve ownedRecipesContainer'a ekle
        GameObject newRecipeUI = Instantiate(recipeTopUIPrefab, ownedRecipesContainer);
        newRecipeUI.GetComponentInChildren<TextMeshProUGUI>().text = recipe.recipeName;
        newRecipeUI.transform.GetChild(0).GetComponent<Image>().sprite = recipe.recipeImage;
    }

    private bool IsRecipeInOwnedContainer(string recipeName)
    {
        foreach (Transform child in ownedRecipesContainer)
        {
            TextMeshProUGUI textComponent = child.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null && textComponent.text == recipeName)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsRecipeInChooseContainer(string recipeName)
    {
        foreach (Transform child in chooseRecipeContainer)
        {
            TextMeshProUGUI textComponent = child.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null && textComponent.text == recipeName)
            {
                return true;
            }
        }
        return false;
    }

    
    private void OnToggleValueChanged(bool isOn, Recipe recipe)
    {
        if (isOn)
        {
            // Tarif seçildi, ownedRecipesContainer'a ekle
            AddRecipeToOwnedContainer(recipe);
            // Seçili tarifleri SaveData'ya ekle
            if (!GameManager.Instance.currentSaveData.selectedRecipeNames.Contains(recipe.recipeName))
            {
                GameManager.Instance.currentSaveData.selectedRecipeNames.Add(recipe.recipeName);
            }
        }
        else
        {
            // Tarif seçimi kaldýrýldý, ownedRecipesContainer'dan çýkar
            RemoveRecipeFromOwnedContainer(recipe.recipeName);
            // Seçili tarifleri SaveData'dan çýkar
            GameManager.Instance.currentSaveData.selectedRecipeNames.Remove(recipe.recipeName);
        }

        // Deðiþiklikleri kaydet
        GameManager.Instance.SaveGame();
    }

    
    private void RemoveRecipeFromOwnedContainer(string recipeName)
    {
        // ownedRecipesContainer içindeki tarifleri kontrol et ve verilen isme sahip olaný kaldýr
        foreach (Transform child in ownedRecipesContainer)
        {
            TextMeshProUGUI textComponent = child.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null && textComponent.text == recipeName)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

   
}
