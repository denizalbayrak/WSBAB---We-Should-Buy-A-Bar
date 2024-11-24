using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public List<Level> levels;

    private int currentLevelIndex = 0;
    public Level currentLevel;
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
    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
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

            // Load the level in OrderManager
            OrderManager.Instance.LoadLevel(currentLevel);

        }
        else
        {
            Debug.LogError("Invalid level index!");
        }
    }
    public Level GetCurrentLevel()
    {
        return currentLevel;
    }
    public void CompleteLevel()
    {
     int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex + 1 > GameManager.Instance.currentSaveData.level)
        {
            GameManager.Instance.currentSaveData.level = nextLevelIndex + 1; // Level numaralarý 1'den baþlýyorsa
            Debug.Log("Unlocked Level: " + GameManager.Instance.currentSaveData.level);
            GameManager.Instance.SaveGame();
        }
        // Baþarý ekraný gösterme vb.
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex + 1 < levels.Count)
        {
            currentLevelIndex++;
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("No more levels!");
        }
    }

}
