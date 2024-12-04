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

   
}