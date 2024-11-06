using UnityEngine;
using UnityEngine.SceneManagement;
using Wsbab.Enums; // CharacterType enumýný kullanmak için
using System.Collections.Generic;

public enum GameState
{
    PreLevel,
    InGame,
    Paused,
    PostLevel,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentGameState = GameState.PreLevel;
    public SaveData currentSaveData;
    public int currentSlot;

    public GameObject maleCharacterPrefab;
    public GameObject femaleCharacterPrefab;
    public CharacterType selectedCharacter = CharacterType.Female;

    public List<PortableObject> defaultInventoryItems; // Default items for each player
    public List<Recipe> availableRecipes; // All recipes available in the game
    public List<Level> levels; // Levels with specific recipes and requirements

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
        currentSaveData.ownedRecipeNames.Add("Beer");
        currentSaveData.ownedRecipeNames.Add("Wine");
        currentSaveData.ownedRecipeNames.Add("Wine1");
        currentSaveData.ownedRecipeNames.Add("Wine2");
        currentSaveData.ownedRecipeNames.Add("Wine3");
        SaveGame();
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame(int slot)
    {
        currentSlot = slot;
        currentSaveData = SaveSystem.LoadGame(slot);

        if (currentSaveData != null)
        {
            selectedCharacter = currentSaveData.selectedCharacter;

            SceneManager.sceneLoaded += OnGameSceneLoaded;
            SceneManager.LoadScene("GameScene");
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
            GameUIManager.Instance.LoadLevelRecipesUI();
            // Instantiate all inventory objects in the game scene
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
        }
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

    public void StartGame()
    {
        currentGameState = GameState.InGame;
        Debug.Log("Oyun baþladý!");
    }
}
