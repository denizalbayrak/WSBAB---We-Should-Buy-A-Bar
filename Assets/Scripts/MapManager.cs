using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public List<Button> levelButtons; 

    private void Start()
    {
        InitializeLevelButtons();
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    private void InitializeLevelButtons()
    {
        int unlockedLevel = GameManager.Instance.currentSaveData.level;

        for (int i = 0; i < levelButtons.Count; i++)
        {
            int levelIndex = i;
            Button button = levelButtons[i];

            if (levelIndex <= unlockedLevel)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }

            LevelButton levelButtonScript = button.GetComponent<LevelButton>();
            if (levelButtonScript != null)
            {
                levelButtonScript.levelIndex = levelIndex;
            }

            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Level " + levelIndex;
            }
        }
    }
}
