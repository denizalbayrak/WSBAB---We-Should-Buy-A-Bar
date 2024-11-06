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

    [Tooltip("Tarif için gerekli objeler")]
    public List<RequiredObject> requiredObjects;
}

[System.Serializable]
public class RequiredObject
{
    [Tooltip("Gereken objenin prefab referansý")]
    public GameObject objectPrefab;

    [Tooltip("Bu objenin kaç tane gerektiði")]
    public int quantity;
}
