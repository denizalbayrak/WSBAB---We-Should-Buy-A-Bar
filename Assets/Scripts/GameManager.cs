using UnityEngine;
using UnityEngine.SceneManagement;
using Wsbab.Enums; // CharacterType enumýný kullanmak için
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

    public CharacterType selectedCharacter = CharacterType.Female; // Varsayýlan olarak Female

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Oyun baþlatýldýðýnda karakter seçimini yükle
            // Eðer isterseniz, burada karakter seçimini yükleyebilirsiniz
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

        // Yeni oyun için varsayýlan deðerleri ayarla
        currentSaveData.level = 1;
        currentSaveData.playTime = 0f;
        currentSaveData.playerName = "Player";

        // Seçilen karakteri save data'ya iþle
        currentSaveData.selectedCharacter = selectedCharacter;

        // Oyunu kaydet
        SaveGame();

        // Oyun sahnesini yükle
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

            // Oyunu kaydet (isteðe baðlý)
            SaveGame();

            // Oyun sahnesini yükle
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // Kayýt bulunamadýysa yeni oyun baþlat
            NewGame(slot);
        }
    }

    public void SaveGame()
    {
        if (currentSaveData != null)
        {
            // currentSaveData'yý güncel oyun durumu ile güncelle
            // Örneðin, playTime veya level gibi

            // Seçilen karakteri save data'ya iþle
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
            // Kayýtlý oyun yoksa, GameManager'daki selectedCharacter'ý kullan
            Debug.Log("SpawnPlayerCharacter - No save data, using selectedCharacter from GameManager: " + selectedCharacter);
        }

        GameObject characterPrefab = (selectedCharacter == CharacterType.Male) ? maleCharacterPrefab : femaleCharacterPrefab;

        // Karakteri istenilen pozisyonda oluþturun
        Vector3 spawnPosition = Vector3.zero; // Spawn pozisyonunu ayarlayýn
        Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
    }

    public void StartGame()
    {
        currentGameState = GameState.InGame;
        Debug.Log("Oyun baþladý!");
    }

}
