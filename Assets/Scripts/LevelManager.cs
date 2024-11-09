using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public List<Level> levels;

    private int currentLevelIndex = 0;
    private Level currentLevel;
    private GameObject currentLevelMapInstance;

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
            // Clean up previous level if any
            if (currentLevelMapInstance != null)
            {
                Destroy(currentLevelMapInstance);
            }

            currentLevel = levels[levelIndex];
            Debug.Log($"Loading Level: {currentLevel.levelName}");

            // Instantiate the level map
            currentLevelMapInstance = Instantiate(currentLevel.levelMapPrefab);
            // Optionally set position, parent, etc.

            // Set up the predetermined recipes
            SetupLevelRecipes();

            // Initialize any other level-specific settings
        }
        else
        {
            Debug.LogError("Invalid level index!");
        }
    }

    private void SetupLevelRecipes()
    {
        // Access the predetermined recipes for the current level
        List<Recipe> levelRecipes = currentLevel.recipes;



    }
}
