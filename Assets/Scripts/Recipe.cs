using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipes/Recipe")]
public class Recipe : ScriptableObject
{
    [Tooltip("Tarifin ad�")]
    public string recipeName;

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
}
