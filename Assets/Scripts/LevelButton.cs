using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndex; // The level number this button represents
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Store the selected level index in GameManager
        GameManager.Instance.selectedLevelIndex = levelIndex - 1; // Adjust if needed

        // Load the GameScene
        SceneManager.LoadScene("GameScene");
    }
}
