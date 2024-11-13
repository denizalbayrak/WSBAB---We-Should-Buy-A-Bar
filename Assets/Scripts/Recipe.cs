using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipes/Recipe")]
public class Recipe : ScriptableObject
{
    [Tooltip("Tarifin ad�")]
    public string recipeName;
    [Tooltip("Tarifin resmi")]
    public Sprite recipeImage;

    [Tooltip("Tarif i�in gerekli objeler")]
    public List<RequiredObject> requiredObjects;
}

[System.Serializable]
public class RequiredObject
{
    [Tooltip("Gereken objenin prefab referans�")]
    public GameObject objectPrefab;

    [Tooltip("Bu objenin ka� tane gerekti�i")]
    public int quantity;

    [Tooltip("Bu obje bara ta��nmal� m�?")]
    public bool moveToBar; // Newly added property

    // Constructor (optional)
    public RequiredObject(GameObject prefab, int qty, bool moveToBar)
    {
        objectPrefab = prefab;
        quantity = qty;
        this.moveToBar = moveToBar;
    }
}