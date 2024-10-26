using UnityEngine;
using UnityEngine.UI;
using Wsbab.Enums; // CharacterType enum�n� kullanmak i�in

public class CharacterSelectionPanel : MonoBehaviour
{
    public Button maleCharacterButton;
    public Button femaleCharacterButton;
    public Button confirmButton;

    public Image maleSelectionHighlight;
    public Image femaleSelectionHighlight;

    private CharacterType selectedCharacter; // Varsay�lan se�imi kald�rd�k

    private void Start()
    {
        // Kaydedilen karakter se�imini y�kle
        LoadCharacterSelection();

        // UI'� g�ncelle
        UpdateSelectionUI();
    }

    public void OnMaleCharacterSelected()
    {
        selectedCharacter = CharacterType.Male;
        UpdateSelectionUI();
        Debug.Log("OnMaleCharacterSelected called. selectedCharacter set to Male.");
    }

    public void OnFemaleCharacterSelected()
    {
        selectedCharacter = CharacterType.Female;
        UpdateSelectionUI();
        Debug.Log("OnFemaleCharacterSelected called. selectedCharacter set to Female.");
    }

    public void OnConfirmSelection()
    {
        // Se�imi kaydet
        SaveCharacterSelection();

        // Paneli kapat
        gameObject.SetActive(false);
    }

    private void UpdateSelectionUI()
    {
        maleSelectionHighlight.enabled = (selectedCharacter == CharacterType.Male);
        femaleSelectionHighlight.enabled = (selectedCharacter == CharacterType.Female);
    }

    private void SaveCharacterSelection()
    {
        if (GameManager.Instance != null)
        {
            // GameManager'daki selectedCharacter de�erini g�ncelle
            GameManager.Instance.selectedCharacter = selectedCharacter;
            Debug.Log("Character selection saved in GameManager: " + selectedCharacter);
        }
        else
        {
            Debug.LogWarning("GameManager instance not found in CharacterSelectionPanel.");
        }
    }

    private void LoadCharacterSelection()
    {
        if (GameManager.Instance != null)
        {
            // E�er GameManager'da selectedCharacter varsa, onu kullan
            selectedCharacter = GameManager.Instance.selectedCharacter;
            Debug.Log("Character selection loaded from GameManager: " + selectedCharacter);
        }
        else
        {
            // Varsay�lan olarak Female se�
            selectedCharacter = CharacterType.Female;
            Debug.LogWarning("GameManager instance not found. Defaulting to Female.");
        }
    }
}
