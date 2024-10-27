using UnityEngine;
using UnityEngine.SceneManagement;
using Wsbab.Enums; // CharacterType enum�n� kullanmak i�in
public enum GameState
{
    PreLevel,
    InGame,
    Paused,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.PreLevel;
    public static GameManager Instance;

    public SaveData currentSaveData;
    public int currentSlot;

    // Character prefabs
    public GameObject maleCharacterPrefab;
    public GameObject femaleCharacterPrefab;

    public CharacterType selectedCharacter = CharacterType.Female; // Varsay�lan olarak Female

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Oyun ba�lat�ld���nda karakter se�imini y�kle
            // E�er isterseniz, burada karakter se�imini y�kleyebilirsiniz
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

        // Se�ilen karakteri save data'ya i�le
        currentSaveData.selectedCharacter = selectedCharacter;

        // Oyunu kaydet
        SaveGame();

        // Oyun sahnesini y�kle
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame(int slot)
    {
        currentSlot = slot;
        currentSaveData = SaveSystem.LoadGame(slot);

        if (currentSaveData != null)
        {
          
            currentSaveData.selectedCharacter = selectedCharacter;
            Debug.Log(selectedCharacter);

            // Oyunu kaydet (iste�e ba�l�)
            SaveGame();

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
            // currentSaveData'y� g�ncel oyun durumu ile g�ncelle
            // �rne�in, playTime veya level gibi

            // Se�ilen karakteri save data'ya i�le
            currentSaveData.selectedCharacter = selectedCharacter;

            SaveSystem.SaveGame(currentSaveData, currentSlot);
        }
    }

    public void SpawnPlayerCharacter()
    {
        if (currentSaveData != null)
        {
            // Save data'daki karakteri kullan
            selectedCharacter = currentSaveData.selectedCharacter;
            Debug.Log("SpawnPlayerCharacter - selectedCharacter from save data: " + selectedCharacter);
        }
        else
        {
            // Kay�tl� oyun yoksa, GameManager'daki selectedCharacter'� kullan
            Debug.Log("SpawnPlayerCharacter - No save data, using selectedCharacter from GameManager: " + selectedCharacter);
        }

        GameObject characterPrefab = (selectedCharacter == CharacterType.Male) ? maleCharacterPrefab : femaleCharacterPrefab;

        // Karakteri istenilen pozisyonda olu�turun
        Vector3 spawnPosition = Vector3.zero; // Spawn pozisyonunu ayarlay�n
        Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
    }

    public void StartGame()
    {
        currentGameState = GameState.InGame;
        Debug.Log("Oyun ba�lad�!");
    }

}
