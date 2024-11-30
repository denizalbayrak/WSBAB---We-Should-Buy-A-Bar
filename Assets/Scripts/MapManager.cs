using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public List<Button> levelButtons; // Seviyelerin butonlarý

    private void Start()
    {
        InitializeLevelButtons();
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

            // Butonun LevelIndex'ini ayarla
            LevelButton levelButtonScript = button.GetComponent<LevelButton>();
            if (levelButtonScript != null)
            {
                levelButtonScript.levelIndex = levelIndex;
            }

            // Buton üzerindeki metni güncelle
            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Level " + levelIndex;
            }
        }
    }
}
