using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [Header("Level Settings")]
    public List<Level> levels;                      // Seviye listesi
    public List<DropPoint> depotDropPoints;         // Depo i�in DropPoint'ler
    public List<DropPoint> barDropPoints;           // Bar i�in DropPoint'ler

    private int currentLevelIndex = 0;
    private Level currentLevel;
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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

                    // �lk olarak depo drop point'lerine yerle�tirme yap
                    foreach (var dropPoint in depotDropPoints)
                    {
                        if (dropPoint.isEmpty) // Sadece bo� drop point'lere yerle�tir
                        {
                            Debug.Log("11111");
                            GameObject newObject = Instantiate(requiredObj.objectPrefab, dropPoint.transform.position, Quaternion.identity);
                            dropPoint.DeliverObject(newObject); // Obje yerle�tir
                            instantiated = true;
                            break;
                        }
                    }

                    //// Depoda yer yoksa bar drop point'lerine yerle�tir
                    //if (!instantiated)
                    //{
                    //    foreach (var dropPoint in barDropPoints)
                    //    {
                    //        if (dropPoint.isEmpty) // Sadece bo� drop point'lere yerle�tir
                    //        {
                    //            GameObject newObject = Instantiate(requiredObj.objectPrefab, dropPoint.transform.position, Quaternion.identity);
                    //            dropPoint.DeliverObject(newObject); // Obje yerle�tir
                    //            instantiated = true;
                    //            break;
                    //        }
                    //    }
                    //}

                    // E�er depo ve bar drop point'lerinde yer yoksa uyar� ver
                    if (!instantiated)
                    {
                        Debug.LogWarning($"No available drop point for object: {requiredObj.objectPrefab.name}");
                    }
                }
            }
        }
    }

    public void UpdateDropPointPlanes(bool isCarrying)
    {
        foreach (var dropPoint in depotDropPoints)
        {
            if (isCarrying)
            {
                dropPoint.TogglePlane(true);
            }

            else
            {
                dropPoint.TogglePlane(false);
            }
        }
        foreach (var dropPoint in barDropPoints)
        {

            if (isCarrying)
            {
                dropPoint.TogglePlane(true);
            }

            else
            {
                dropPoint.TogglePlane(false);
            }
        }

    }
}
