using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public List<Level> levels;

    public int currentLevelIndex = 0;
    public Level currentLevel;
    private GameObject currentLevelMapInstance;
    private GameObject currentLevelInstance;
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
        Debug.Log("Loading Level:" + levelIndex); 
        currentLevelIndex = levelIndex;
        if (levelIndex >= 0 && levelIndex < levels.Count)
        {
            UnloadCurrentLevel();
            currentLevel = levels[levelIndex];
            Debug.Log($"Loading Level: {currentLevel.levelName}");
            // Clean up previous level if any
            if (currentLevel.levelMapPrefab != null)
            {
                currentLevelInstance = Instantiate(currentLevel.levelMapPrefab);
            }
            else
            {
                Debug.LogError("Current level prefab is null!");
            }

            // Load the level in OrderManager
            OrderManager.Instance.ResetOrderManager();
            OrderManager.Instance.LoadLevel(currentLevel);
        }
        else
        {
            Debug.LogError("Invalid level index!");
        }
    }
    public void UnloadCurrentLevel()
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }
    }
    public Level GetCurrentLevel()
    {
        return currentLevel;
    }
    public void CompleteLevel()
    {
     int nextLevelIndex = GameManager.Instance.currentSaveData.level + 1;
        Debug.Log("currentLevelIndex + " + currentLevelIndex);
        Debug.Log("GameManager.Instance.currentSaveData.level + " + GameManager.Instance.currentSaveData.level);
        if (nextLevelIndex + 1 > GameManager.Instance.currentSaveData.level)
        {
            GameManager.Instance.currentSaveData.level = nextLevelIndex; // Level numaralarý 1'den baþlýyorsa
            Debug.Log("Unlocked Level: " + GameManager.Instance.currentSaveData.level);
            GameManager.Instance.SaveGame();
        }
        // Baþarý ekraný gösterme vb.
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex < levels.Count)
        {
            LoadLevel(nextLevelIndex);
            GameManager.Instance.selectedLevelIndex = nextLevelIndex;
        }
        else
        {
            Debug.Log("No more levels!");
            // Oyunun sonuna geldiniz, isterseniz bir bitiþ ekraný gösterebilirsiniz
        }
    }

}
