using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionPanel : MonoBehaviour
{
    public Button maleCharacterButton;
    public Button femaleCharacterButton;
    public Button confirmButton;

    public Image maleSelectionHighlight;
    public Image femaleSelectionHighlight;

    private CharacterType selectedCharacter = CharacterType.Female; // Varsayýlan seçim

    private void Start()
    {
        // Kaydedilmiþ karakter seçimini yükle
        LoadCharacterSelection();

        // UI'ý güncelle
        UpdateSelectionUI();
    }

    public void OnMaleCharacterSelected()
    {
        selectedCharacter = CharacterType.Male;
        UpdateSelectionUI();
    }

    public void OnFemaleCharacterSelected()
    {
        selectedCharacter = CharacterType.Female;
        UpdateSelectionUI();
    }

    public void OnConfirmSelection()
    {
        // Seçimi kaydet
        SaveCharacterSelection();

        // Paneli kapat
        gameObject.SetActive(false);

        // Gerekirse diðer UI veya oyun öðelerini güncelle
    }

    private void UpdateSelectionUI()
    {
        maleSelectionHighlight.enabled = (selectedCharacter == CharacterType.Male);
        femaleSelectionHighlight.enabled = (selectedCharacter == CharacterType.Female);
    }

    private void SaveCharacterSelection()
    {
        PlayerPrefs.SetInt("SelectedCharacter", (int)selectedCharacter);
        PlayerPrefs.Save();
        Debug.Log("Character saved: " + selectedCharacter);

        if (GameManager.Instance != null && GameManager.Instance.currentSaveData != null)
        {
            GameManager.Instance.currentSaveData.selectedCharacter = selectedCharacter;
            GameManager.Instance.SaveGame();
            Debug.Log("Character saved in GameManager: " + selectedCharacter);
        }
    }


    private void LoadCharacterSelection()
    {
        // Seçimi PlayerPrefs veya kayýt sisteminizden yükleyin
        if (GameManager.Instance != null && GameManager.Instance.currentSaveData != null)
        {
            selectedCharacter = GameManager.Instance.currentSaveData.selectedCharacter;
        }
        else if (PlayerPrefs.HasKey("SelectedCharacter"))
        {
            selectedCharacter = (CharacterType)PlayerPrefs.GetInt("SelectedCharacter");
        }
        else
        {
            selectedCharacter = CharacterType.Female; // Varsayýlan olarak kadýn karakter
        }
    }
}
