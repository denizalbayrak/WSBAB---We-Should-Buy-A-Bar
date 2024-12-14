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
    private GameObject playerInstance;
    private bool isSubscribed = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnGameSceneLoaded; // Aboneliði burada yapýyoruz
            isSubscribed = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (isSubscribed)
        {
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
            isSubscribed = false;
        }
    }
    public void NewGame(int slot)
    {
        currentSlot = slot;
        currentSaveData = new SaveData
        {
            level = 0,
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
            SpawnPlayerCharacter();
            LevelManager.Instance.LoadLevel(selectedLevelIndex);

            GameUIManager.Instance.StartCountdown();
            Time.timeScale = 1f;            
            GameUIManager.Instance.ResetUI();
        }
    }


    public void RestartLevel()
    {
        LevelManager.Instance.UnloadCurrentLevel();
        DestroyPlayerCharacter();
        LevelManager.Instance.LoadLevel(LevelManager.Instance.currentLevelIndex);
        SpawnPlayerCharacter();
        currentGameState = GameState.InGame;
        GameUIManager.Instance.StartCountdown();
        Time.timeScale = 0f;
    }


    public void SaveGame()
    {
        if (currentSaveData != null)
        {
            currentSaveData.selectedCharacter = selectedCharacter;
            SaveSystem.SaveGame(currentSaveData, currentSlot);
            Debug.Log("currentSaveData " + currentSaveData);
        }
    }

    public void SpawnPlayerCharacter()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        GameObject characterPrefab = (selectedCharacter == CharacterType.Male) ? maleCharacterPrefab : femaleCharacterPrefab;
        Vector3 spawnPosition = Vector3.zero; // Adjust spawn position as needed
        playerInstance = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
    }
    public void DestroyPlayerCharacter()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
            playerInstance = null;
        }
    }


}
