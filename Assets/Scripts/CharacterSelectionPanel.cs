using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionPanel : MonoBehaviour
{
    public Button maleCharacterButton;
    public Button femaleCharacterButton;
    public Button confirmButton;

    public Image maleSelectionHighlight;
    public Image femaleSelectionHighlight;

    private CharacterType selectedCharacter = CharacterType.Female; // Varsay�lan se�im

    private void Start()
    {
        // Kaydedilmi� karakter se�imini y�kle
        LoadCharacterSelection();

        // UI'� g�ncelle
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
        // Se�imi kaydet
        SaveCharacterSelection();

        // Paneli kapat
        gameObject.SetActive(false);

        // Gerekirse di�er UI veya oyun ��elerini g�ncelle
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
        // Se�imi PlayerPrefs veya kay�t sisteminizden y�kleyin
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
            selectedCharacter = CharacterType.Female; // Varsay�lan olarak kad�n karakter
        }
    }
}
