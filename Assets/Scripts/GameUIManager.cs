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
    public Transform ownedRecipesContainer; // Horizontal layout container
    public Transform chooseRecipeContainer; // Panel listing all recipes
    public GameObject WarningPanel; // Reference to the warning panel
    public TextMeshProUGUI WarningPanelText; // Reference to the warning panel

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
        LoadChooseRecipesUI();
    }

    public void LoadChooseRecipesUI()
    {
        List<string> ownedRecipes = GameManager.Instance.currentSaveData.ownedRecipeNames;

        // Clear existing recipe UI elements
        foreach (Transform child in chooseRecipeContainer)
        {
            Destroy(child.gameObject);
        }

        // Check if any recipes have been selected previously
        bool hasSelectedRecipes = GameManager.Instance.currentSaveData.selectedRecipeNames != null &&
                                  GameManager.Instance.currentSaveData.selectedRecipeNames.Count > 0;

        foreach (var recipeName in ownedRecipes)
        {
            Recipe recipe = ItemHolder.Instance.GetRecipe(recipeName);
            if (recipe != null)
            {
                GameObject newRecipeToggleUI = Instantiate(recipeToggleUIPrefab, chooseRecipeContainer);
                newRecipeToggleUI.GetComponentInChildren<TextMeshProUGUI>().text = recipe.recipeName;
                newRecipeToggleUI.transform.GetChild(0).GetComponent<Image>().sprite = recipe.recipeImage;

                Toggle toggle = newRecipeToggleUI.GetComponentInChildren<Toggle>();
                if (toggle != null)
                {
                    toggle.onValueChanged.AddListener(isOn => OnToggleValueChanged(isOn, recipe));

                    bool isSelected;

                    if (!hasSelectedRecipes)
                    {
                        // If no selections have been made, default to "Beer" only
                        isSelected = recipe.recipeName == "Beer";
                        if (isSelected)
                        {
                            // Add "Beer" to selectedRecipeNames if not already added
                            if (!GameManager.Instance.currentSaveData.selectedRecipeNames.Contains(recipe.recipeName))
                            {
                                GameManager.Instance.currentSaveData.selectedRecipeNames.Add(recipe.recipeName);
                            }
                        }
                    }
                    else
                    {
                        // Use previously saved selections
                        isSelected = GameManager.Instance.currentSaveData.selectedRecipeNames.Contains(recipe.recipeName);
                    }

                    toggle.isOn = isSelected;

                    // Add to ownedRecipesContainer if selected
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

        // After setting up toggles, check if no recipes are selected
        if (GameManager.Instance.currentSaveData.selectedRecipeNames.Count == 0)
        {
            // Show warning panel
            WarningPanel.SetActive(true);
            WarningPanelText.text = "You must choose at least one recipe.";
        }
        else
        {
            // Hide warning panel
            WarningPanel.SetActive(false);
        }
    }

    private void AddRecipeToOwnedContainer(Recipe recipe)
    {
        // Do not add if already present
        if (IsRecipeInOwnedContainer(recipe.recipeName))
        {
            return;
        }

        // Create a new UI element and add it to ownedRecipesContainer
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

    private void OnToggleValueChanged(bool isOn, Recipe recipe)
    {
        if (isOn)
        {
            // Recipe selected, add to ownedRecipesContainer
            AddRecipeToOwnedContainer(recipe);
            // Add to selectedRecipeNames in SaveData
            if (!GameManager.Instance.currentSaveData.selectedRecipeNames.Contains(recipe.recipeName))
            {
                GameManager.Instance.currentSaveData.selectedRecipeNames.Add(recipe.recipeName);
            }
        }
        else
        {
            // Recipe deselected, remove from ownedRecipesContainer
            RemoveRecipeFromOwnedContainer(recipe.recipeName);
            // Remove from selectedRecipeNames in SaveData
            GameManager.Instance.currentSaveData.selectedRecipeNames.Remove(recipe.recipeName);
        }

        // Save changes
        GameManager.Instance.SaveGame();

        // Check if no recipes are selected
        if (GameManager.Instance.currentSaveData.selectedRecipeNames.Count == 0)
        {
            // Show warning panel
            WarningPanel.SetActive(true);
            WarningPanelText.text = "You must choose at least one recipe.";
        }
        else
        {
            // Hide warning panel
            WarningPanel.SetActive(false);
        }
    }

    private void RemoveRecipeFromOwnedContainer(string recipeName)
    {
        // Check ownedRecipesContainer and remove the recipe with the given name
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
