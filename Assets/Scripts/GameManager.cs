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

        SaveGame();
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame(int slot)
    {
        currentSlot = slot;
        currentSaveData = SaveSystem.LoadGame(slot);

        if (currentSaveData != null)
        {
            selectedCharacter = currentSaveData.selectedCharacter;
            LoadOwnedRecipesFromSaveData(); // Load owned recipes by names

           
            SceneManager.LoadScene("GameScene");
            SceneManager.sceneLoaded += OnGameSceneLoaded;
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
            // Instantiate all inventory objects in the game scene
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
        }
    }

   

    private List<PortableObject> GetItemsFromOwnedRecipes()
    {
        List<PortableObject> items = new List<PortableObject>();
        HashSet<string> existingItems = new HashSet<string>();

        // Loop through each owned recipe and add unique items
        foreach (var recipe in GetOwnedRecipes())
        {
            foreach (var item in recipe.requiredObjects)
            {
                if (!existingItems.Contains(item.objectPrefab.name))
                {
                    items.Add(item.objectPrefab.GetComponent<PortableObject>());
                    existingItems.Add(item.objectPrefab.name);
                }
            }
        }
        return items;
    }

    private List<Recipe> GetOwnedRecipes()
    {
        List<Recipe> ownedRecipes = new List<Recipe>();
        foreach (var recipeName in currentSaveData.ownedRecipeNames)
        {
            Recipe recipe = availableRecipes.Find(r => r.name == recipeName);
            if (recipe != null)
            {
                ownedRecipes.Add(recipe);
            }
        }
        return ownedRecipes;
    }

    private void LoadOwnedRecipesFromSaveData()
    {
        foreach (var recipeName in currentSaveData.ownedRecipeNames)
        {
            Recipe recipe = availableRecipes.Find(r => r.name == recipeName);
            if (recipe != null)
            {
                currentSaveData.ownedRecipeNames.Add(recipe.name);
            }
        }
    }

    public void SaveGame()
    {
        if (currentSaveData != null)
        {
            currentSaveData.selectedCharacter = selectedCharacter;
            currentSaveData.ownedRecipeNames = GetOwnedRecipeNames(); // Save owned recipes by names
            SaveSystem.SaveGame(currentSaveData, currentSlot);
        }
    }

    private List<string> GetOwnedRecipeNames()
    {
        List<string> recipeNames = new List<string>();
        foreach (var recipe in GetOwnedRecipes())
        {
            recipeNames.Add(recipe.name);
        }
        return recipeNames;
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
