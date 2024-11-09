using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public List<Recipe> recipes;
    public GameObject levelMapPrefab; // Reference to the level's map prefab
}
