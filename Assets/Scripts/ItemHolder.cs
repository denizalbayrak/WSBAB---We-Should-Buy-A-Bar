using System.Collections.Generic;
using UnityEngine;
public enum GlassType
{
    Beer,
    Wine,
    Whiskey
}

public class ItemHolder : MonoBehaviour
{
    public static ItemHolder Instance;
    public List<Recipe> Recipes = new List<Recipe>();

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

    // Tarifleri yükle (SaveData üzerinden alýnabilir)
    public void LoadRecipesFromSaveData(List<string> recipeNames)
    {
        Recipes.Clear();
        foreach (string recipeName in recipeNames)
        {
            Recipe recipe = GetRecipeByName(recipeName);
            if (recipe != null)
            {
                Recipes.Add(recipe);
            }
        }
    }

    // Helper function to get Recipe by name
    private Recipe GetRecipeByName(string recipeName)
    {
        return GameManager.Instance.availableRecipes.Find(r => r.recipeName == recipeName);
    }

    // Tarifi kontrol et
    public Recipe GetRecipe(string recipeName)
    {
        return Recipes.Find(r => r.recipeName == recipeName);
    }
}
