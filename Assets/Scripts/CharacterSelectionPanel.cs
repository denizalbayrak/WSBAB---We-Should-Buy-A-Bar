using UnityEngine;
using UnityEngine.UI;
using Wsbab.Enums; 

public class CharacterSelectionPanel : MonoBehaviour
{
    public Button maleCharacterButton;
    public Button femaleCharacterButton;
    public Button confirmButton;

    public Image maleSelectionHighlight;
    public Image femaleSelectionHighlight;

    private CharacterType selectedCharacter; 

    private void Start()
    {
        LoadCharacterSelection();

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
        SaveCharacterSelection();

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
            selectedCharacter = GameManager.Instance.selectedCharacter;
            Debug.Log("Character selection loaded from GameManager: " + selectedCharacter);
        }
        else
        {
            selectedCharacter = CharacterType.Female;
            Debug.LogWarning("GameManager instance not found. Defaulting to Female.");
        }
    }
}
