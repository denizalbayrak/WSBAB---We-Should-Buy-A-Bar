using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/Level")]
public class Level : ScriptableObject
{
    [Tooltip("Seviyenin adý")]
    public string levelName;

    [Tooltip("Seviyedeki tarifler")]
    public List<Recipe> recipes;
}
