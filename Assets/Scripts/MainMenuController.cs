using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public GameObject saveSelectionPanel;
    public Button playButton;
    public TextMeshProUGUI playButtonText;

    public void OpenCharacterCustomization()
    {
        SceneManager.LoadScene("CharacterCustomizationScene");
    }
    private void Start()
    {
        UpdatePlayButtonText();
    }
    private void UpdatePlayButtonText()
    {
        bool hasSave = SaveSystem.HasAnySave();

        if (hasSave)
        {
            playButtonText.text = "Continue";
        }
        else
        {
            playButtonText.text = "Play";
        }
    }

   
    public void PlayOrContinue()
    {
        saveSelectionPanel.SetActive(true);
    }
    public void NewGame()
    {
        saveSelectionPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

