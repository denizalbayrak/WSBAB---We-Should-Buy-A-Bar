using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public List<Level> levels;                      // List of levels
    public List<DropPoint> depotDropPoints;         // DropPoints for Depot
    public List<DropPoint> barDropPoints;           // DropPoints for Bar

    private int currentLevelIndex = 0;
    private Level currentLevel;
    private List<PortableObject> requiredBarObjects = new List<PortableObject>(); // Objects to be moved to the Bar
    private List<PortableObject> instantiatedObjects = new List<PortableObject>(); // Track instantiated PortableObjects

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optionally, you can keep the LevelManager alive across scenes
            // DontDestroyOnLoad(gameObject);
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
            // Instantiate required objects at level start
        }
        else
        {
            Debug.LogError("Invalid level index!");
        }
    }

    /// <summary>
    /// Instantiates required objects based on the player's selected recipes.
    /// </summary>
    public void InstantiateRequiredObjects()
    {

        // Remove previously instantiated objects
        foreach (var portableObject in instantiatedObjects)
        {
            if (portableObject != null)
            {
                // Remove from requiredBarObjects if necessary
                requiredBarObjects.Remove(portableObject);

                // Remove from drop point (which will destroy the object)
                DropPoint dropPoint = portableObject.GetComponentInParent<DropPoint>();
                if (dropPoint != null)
                {
                    dropPoint.RemoveObject();
                }
                else
                {
                    // If not attached to a drop point, destroy directly
                    Destroy(portableObject.gameObject);
                }
            }
        }
        instantiatedObjects.Clear();

        List<string> selectedRecipeNames = GameManager.Instance.currentSaveData.selectedRecipeNames;

        if (selectedRecipeNames == null || selectedRecipeNames.Count == 0)
        {
            Debug.LogWarning("No selected recipes found. Cannot instantiate required objects.");
            return;
        }
        foreach (var recipeName in selectedRecipeNames)
        {
            Recipe recipe = ItemHolder.Instance.GetRecipe(recipeName);
            if (recipe != null)
            {
                foreach (var requiredObj in recipe.requiredObjects)
                {
                    for (int i = 0; i < requiredObj.quantity; i++)
                    {
                        bool instantiated = false;

                        // Find an empty depot drop point and place the object
                        foreach (var dropPoint in depotDropPoints)
                        {
                            if (dropPoint.isEmpty)
                            {
                                GameObject newObject = Instantiate(requiredObj.objectPrefab, dropPoint.transform.position, Quaternion.identity);
                                PortableObject portableObject = newObject.GetComponent<PortableObject>();

                                if (portableObject != null)
                                {
                                    dropPoint.DeliverObject(newObject);
                                    instantiatedObjects.Add(portableObject);

                                    // If the object needs to be moved to the bar, add it to the list
                                    if (requiredObj.moveToBar)
                                    {
                                        requiredBarObjects.Add(portableObject);
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning("PortableObject component missing on instantiated object.");
                                    Destroy(newObject);
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
            else
            {
                Debug.LogWarning($"Recipe not found: {recipeName}");
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
