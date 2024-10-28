using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public List<Level> levels;              // Seviye listesi
    public List<DropPoint> dropPoints;      // Tüm DropPoint'ler

    private int currentLevelIndex = 0;
    private Level currentLevel;

    private void Start()
    {
        if (levels.Count > 0)
        {
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.LogError("No levels assigned in LevelManager!");
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Count)
        {
            currentLevel = levels[levelIndex];
            Debug.Log($"Loading Level: {currentLevel.levelName}");
            InstantiateRequiredObjects(currentLevel);
        }
        else
        {
            Debug.LogError("Invalid level index!");
        }
    }

    private void InstantiateRequiredObjects(Level level)
    {
        foreach (var recipe in level.recipes)
        {
            foreach (var requiredObj in recipe.requiredObjects)
            {
                for (int i = 0; i < requiredObj.quantity; i++)
                {
                    bool instantiated = false;

                    // Objeleri sadece drop point'lerin içinde yarat
                    foreach (var dropPoint in dropPoints)
                    {
                        if (dropPoint.IsEmpty)
                        {
                            GameObject newObject = Instantiate(requiredObj.objectPrefab, dropPoint.transform.position, Quaternion.identity);
                            dropPoint.DeliverObject(newObject);  // DropPoint'e obje yerleþtir
                            instantiated = true;
                            break;
                        }
                    }

                    // Eðer tüm drop point'ler doluysa uyarý ver
                    if (!instantiated)
                    {
                        Debug.LogWarning($"No available drop point for object: {requiredObj.objectPrefab.name}");
                    }
                }
            }
        }
    }
}
