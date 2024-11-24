using UnityEngine;
using UnityEngine.SceneManagement;
using Wsbab.Enums; // CharacterType enumýný kullanmak için
using System.Collections.Generic;
using TMPro;
using System.Collections; // Eðer TextMeshPro kullanýyorsanýz

public enum GameState
{
    InGame,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentGameState = GameState.InGame;
    public SaveData currentSaveData;
    public int currentSlot;

    public GameObject maleCharacterPrefab;
    public GameObject femaleCharacterPrefab;
    public CharacterType selectedCharacter = CharacterType.Female;

    public List<PortableObject> defaultInventoryItems; // Default items for each player
    public List<Recipe> availableRecipes; // All recipes available in the game
    public List<Level> levels; // Levels with specific recipes and requirements
    public int selectedLevelIndex = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnGameSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnGameSceneLoaded;
    }
    public void NewGame(int slot)
    {
        currentSlot = slot;
        currentSaveData = new SaveData
        {
            level = 1,
            playTime = 0f,
            playerName = "Player",
            selectedCharacter = selectedCharacter,
            ownedRecipeNames = new List<string>() // Store recipe names for owned recipes
        };
        SaveGame();
        //SceneManager.sceneLoaded += OnGameSceneLoaded;
        //SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene("MapScene");
    }

    public void LoadGame(int slot)
    {
        currentSlot = slot;
        currentSaveData = SaveSystem.LoadGame(slot);

        if (currentSaveData != null)
        {
            selectedCharacter = currentSaveData.selectedCharacter;

            //SceneManager.sceneLoaded += OnGameSceneLoaded;
            //SceneManager.LoadScene("GameScene");
            SceneManager.LoadScene("MapScene");
        }
        else
        {
            NewGame(slot);
        }
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            // Ensure LevelManager is initialized in GameScene

            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.LoadLevel(selectedLevelIndex);
            }
            else
            {
                Debug.LogError("LevelManager not found in GameScene.");
            }

            GameUIManager.Instance.StartCountdown();

            SceneManager.sceneLoaded -= OnGameSceneLoaded;
        }
    }


    public void RestartLevel()
    {
        Time.timeScale = 1f;
        currentGameState = GameState.InGame;
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }


    public void SaveGame()
    {
        if (currentSaveData != null)
        {
            currentSaveData.selectedCharacter = selectedCharacter;
            SaveSystem.SaveGame(currentSaveData, currentSlot);
        }
    }

    public void SpawnPlayerCharacter()
    {
        GameObject characterPrefab = (selectedCharacter == CharacterType.Male) ? maleCharacterPrefab : femaleCharacterPrefab;
        Vector3 spawnPosition = Vector3.zero; // Adjust spawn position as needed
        Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
    }

    
    
}
