using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipes/Recipe")]
public class Recipe : ScriptableObject
{
    [Tooltip("Tarifin adý")]
    public string recipeName;
    [Tooltip("Tarifin resmi")]
    public Sprite recipeImage;

}

[System.Serializable]
public class RequiredObject
{
    [Tooltip("Gereken objenin prefab referansý")]
    public GameObject objectPrefab;

    [Tooltip("Bu objenin kaç tane gerektiði")]
    public int quantity;

    [Tooltip("Bu obje bara taþýnmalý mý?")]
    public bool moveToBar; // Newly added property

   
}