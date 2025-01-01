using UnityEngine;
using UnityEngine.SceneManagement;
using Wsbab.Enums; 
using System.Collections.Generic;
using TMPro;
using System.Collections; 

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

    public List<PortableObject> defaultInventoryItems; 
    public List<Recipe> availableRecipes; 
    public List<Level> levels; 
    public int selectedLevelIndex = 0;
    private GameObject playerInstance;
    private bool isSubscribed = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnGameSceneLoaded; 
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
            ownedRecipeNames = new List<string>() 
        };
        SaveGame();
        SceneManager.LoadScene("MapScene");
    }

    public void LoadGame(int slot)
    {
        currentSlot = slot;
        currentSaveData = SaveSystem.LoadGame(slot);

        if (currentSaveData != null)
        {
            selectedCharacter = currentSaveData.selectedCharacter;

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
            if (selectedLevelIndex <= 4)
            {
                GameUIManager.Instance.OpenTutorial();
            }
            else
            {
                GameUIManager.Instance.StartCountdown();
            }
            Time.timeScale = 1f;
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
        Vector3 spawnPosition = Vector3.zero;
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
