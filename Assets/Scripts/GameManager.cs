using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SaveData currentSaveData;
    public int currentSlot;

    // Character prefabs
    public GameObject maleCharacterPrefab;
    public GameObject femaleCharacterPrefab;

    private void Awake()
    {
        // Singleton pattern
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
        currentSaveData = new SaveData();
        // Yeni oyun i�in varsay�lan de�erleri ayarla
        currentSaveData.level = 1;
        currentSaveData.playTime = 0f;
        currentSaveData.playerName = "Player";

        // Varsay�lan karakter olarak Female se�
        currentSaveData.selectedCharacter = CharacterType.Female;

        // Oyunu kaydet
        SaveSystem.SaveGame(currentSaveData, currentSlot);

        // Oyun sahnesini y�kle
        SceneManager.LoadScene("GameScene");
    }
    public void SpawnPlayerCharacter()
    {
        CharacterType selectedCharacter = CharacterType.Female; // Varsay�lan

        if (currentSaveData != null && !string.IsNullOrEmpty(currentSaveData.playerName))
        {
            selectedCharacter = currentSaveData.selectedCharacter;
        }
        else if (PlayerPrefs.HasKey("SelectedCharacter"))
        {
            selectedCharacter = (CharacterType)PlayerPrefs.GetInt("SelectedCharacter");
        }

        GameObject characterPrefab = (selectedCharacter == CharacterType.Male) ? maleCharacterPrefab : femaleCharacterPrefab;

        // Karakteri istenilen pozisyonda olu�turun
        Vector3 spawnPosition = Vector3.zero; // Spawn pozisyonunu ayarlay�n
        Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
    }


    public void LoadGame(int slot)
    {
        currentSlot = slot;
        currentSaveData = SaveSystem.LoadGame(slot);

        if (currentSaveData != null)
        {
            // Oyun sahnesini y�kle
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // Kay�t bulunamad�ysa yeni oyun ba�lat
            NewGame(slot);
        }
    }

    public void SaveGame()
    {
        if (currentSaveData != null)
        {
            // Update currentSaveData with current game state

            SaveSystem.SaveGame(currentSaveData, currentSlot);
        }
    }
}
