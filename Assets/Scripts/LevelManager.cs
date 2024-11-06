using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public List<Level> levels;                      // Seviye listesi
    public List<DropPoint> depotDropPoints;         // Depo için DropPoint'ler
    public List<DropPoint> barDropPoints;           // Bar için DropPoint'ler

    private int currentLevelIndex = 0;
    private Level currentLevel;
    private List<PortableObject> requiredBarObjects = new List<PortableObject>(); // Bar'a taþýnmasý gereken objeler

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

    /// <summary>
    /// Level gereksinimlerine göre gerekli objeleri yaratýr ve depoya yerleþtirir.
    /// </summary>
    private void InstantiateRequiredObjects(Level level)
    {
        foreach (var recipe in level.recipes)
        {
            foreach (var requiredObj in recipe.requiredObjects)
            {
                for (int i = 0; i < requiredObj.quantity; i++)
                {
                    bool instantiated = false;

                    // Depo alanýnda boþ drop point bul ve objeyi yerleþtir
                    foreach (var dropPoint in depotDropPoints)
                    {
                        if (dropPoint.isEmpty)
                        {
                            GameObject newObject = Instantiate(requiredObj.objectPrefab, dropPoint.transform.position, Quaternion.identity);
                            PortableObject portableObject = newObject.GetComponent<PortableObject>();
                            if (portableObject != null)
                            {
                                dropPoint.DeliverObject(newObject);

                                                              
                            }
                            instantiated = true;
                            break;
                        }
                    }

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
            dropPoint.TogglePlane(isCarrying);
        }
        foreach (var dropPoint in barDropPoints)
        {
            dropPoint.TogglePlane(isCarrying);
        }
    }

    public bool AreRequiredObjectsMovedToBar()
    {
        foreach (var obj in requiredBarObjects)
        {
            bool isInBar = false;
            foreach (var barDropPoint in barDropPoints)
            {
                if (!barDropPoint.isEmpty && barDropPoint.deliveredObject == obj.gameObject)
                {
                    isInBar = true;
                    break;
                }
            }
            if (!isInBar)
            {
                return false;
            }
        }
        return true;
    }
}
