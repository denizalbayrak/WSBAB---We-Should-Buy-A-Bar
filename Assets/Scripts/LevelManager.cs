using UnityEngine;
using System.Collections.Generic;
public enum GlassType
{
    Beer,
    Wine,
    Mojito,
    Mimosa,
    Whiskey
}
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
        currentLevelIndex = levelIndex;
        Debug.Log("levelIndex + " + levelIndex);
        Debug.Log("currentLevelIndex + " + currentLevelIndex);

        if (levelIndex >= 0 && levelIndex < levels.Count)
        {
            UnloadCurrentLevel();
            currentLevel = levels[levelIndex];
            Debug.Log($"Loading Level: {currentLevel.levelName}");
            if (currentLevel.levelMapPrefab != null)
            {
                currentLevelInstance = Instantiate(currentLevel.levelMapPrefab);
            }
            else
            {
                Debug.LogError("Current level prefab is null!");
            }

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
        int nextLevelIndex = currentLevelIndex + 1;
        Debug.Log("nextLevelIndex " + nextLevelIndex);
        Debug.Log("GameManager.Instance.currentSaveData.level " + GameManager.Instance.currentSaveData.level);
        if (nextLevelIndex > GameManager.Instance.currentSaveData.level)
        {
            GameManager.Instance.currentSaveData.level = nextLevelIndex;
            Debug.Log("Unlocked Level: " + GameManager.Instance.currentSaveData.level);
            GameManager.Instance.SaveGame();
        }
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
        }
    }

}
