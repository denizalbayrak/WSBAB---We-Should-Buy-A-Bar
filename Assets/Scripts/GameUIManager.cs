using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    public static GameManager gameManager;

    public GameObject WarningPanel; // Reference to the warning panel
    public TextMeshProUGUI WarningPanelText; // Reference to the warning panel

    public float countdownTime = 3f;
    public TextMeshProUGUI countdownText;
    public GameObject pauseMenuUI;
    public TextMeshProUGUI levelTimerText;
    public Slider levelTimerSlider;

    private float levelDuration;
    private float timeRemaining;

    public GameObject endPanel;
    public GameObject successPanel;
    public GameObject failurePanel;
    public Button nextLevelButton;
    public GameObject[] stars;

    private int finalScore;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameManager = GameManager.Instance;
    }
    // Pause fonksiyonlarý
    public void PauseGame()
    {

        if (gameManager.currentGameState == GameState.InGame)
        {
            Time.timeScale = 0f;
            gameManager.currentGameState = GameState.Paused;
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(true);
            }
        }
    }

    public void ResumeGame()
    {
        if (gameManager.currentGameState == GameState.Paused)
        {
            Time.timeScale = 1f;
            gameManager.currentGameState = GameState.InGame;
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }
        }
    }


    public void RestartLevel()
    {
        gameManager.RestartLevel();
    }


    public void ExitLevel()
    {
        Time.timeScale = 1f;
        gameManager.currentGameState = GameState.InGame;
        SceneManager.LoadScene("MainMenuScene"); // Ana menü sahnenizin adýný yazýn
    }
    public void StartLevelTimer(float duration)
    {
        levelDuration = duration;
        timeRemaining = duration;

        // Slider ayarlarý
        if (levelTimerSlider != null)
        {
            levelTimerSlider.maxValue = duration;
            levelTimerSlider.value = duration;
        }

        StartCoroutine(UpdateLevelTimer());
    }

    private IEnumerator Countdown()
    {
        // Oyunu duraklat
        Time.timeScale = 0f;
        countdownText.transform.parent.gameObject.SetActive(true);
        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            // Metni güncelle
            if (countdownText != null)
            {
                countdownText.text = Mathf.CeilToInt(remainingTime).ToString();
            }

            // Bir sonraki kareye kadar bekle
            yield return new WaitForSecondsRealtime(1f);

            remainingTime--;
        }

        // Geri sayým bittiðinde
        if (countdownText != null)
        {
            countdownText.text = "GO!";
        }
        yield return new WaitForSecondsRealtime(1f);

        // Oyunu baþlat
        Time.timeScale = 1f;
        countdownText.transform.parent.gameObject.SetActive(false);
        countdownText.transform.parent.parent.gameObject.SetActive(false);
        gameManager.currentGameState = GameState.InGame;

        // Level süresini baþlat
        StartLevelTimer(LevelManager.Instance.currentLevel.levelDuration);
        Debug.Log("GameStarted!");
    }
    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }
    private IEnumerator UpdateLevelTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            // Metni güncelle
            if (levelTimerText != null)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);
                levelTimerText.text = $"{minutes:00}:{seconds:00}";
            }

            // Slider'ý güncelle
            if (levelTimerSlider != null)
            {
                levelTimerSlider.value = timeRemaining;
            }

            yield return null;
        }

        // Süre dolduðunda bir þey yap
        if (levelTimerText != null)
        {
            levelTimerText.text = "00:00";
            ShowEndPanel(OrderManager.Instance.currentScore);
        }

        Debug.Log("Level time is over!");
    }

    public void ShowEndPanel(int score)
    {
        OrderManager.Instance.StopLevel();
        finalScore = score;
        endPanel.SetActive(true);

        Level currentLevel = LevelManager.Instance.GetCurrentLevel();
        if (currentLevel == null)
        {
            Debug.LogError("Current level is null!");
            return;
        }

        // Yýldýz sayýsýný hesapla
        int starCount = CalculateStarCount(score, currentLevel);

        // Yýldýz görsellerini güncelle
        for (int i = 0; i < stars.Length; i++)
        {
            if (i == 0)
            {
                stars[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentLevel.targetScore.ToString();
            }
            if (i == 1)
            {
                stars[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentLevel.twoStarScore.ToString();
            }
            if (i == 2)
            {
                stars[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentLevel.threeStarScore.ToString();
            }

            Image starImage = stars[i].GetComponent<Image>();
            if (starImage != null)
            {
                if (i < starCount)
                {
                    starImage.color = Color.yellow;
                }
                else
                {
                    starImage.color = Color.gray;
                }
            }
        }

        // Baþarýlý veya baþarýsýz UI'ý göster
        if (starCount > 0)
        {
            LevelManager.Instance.CompleteLevel();
            successPanel.SetActive(true);
            failurePanel.SetActive(false);

            // Next Level butonu sadece baþarýlý olduðunda aktif
            if (nextLevelButton != null)
            {
                nextLevelButton.interactable = true;
            }
        }
        else
        {
            successPanel.SetActive(false);
            failurePanel.SetActive(true);

            // Baþarýsýz olursa next level butonu pasif
            if (nextLevelButton != null)
            {
                nextLevelButton.interactable = false;
            }
        }
    }
    private int CalculateStarCount(int score, Level currentLevel)
    {
        if (score >= currentLevel.threeStarScore)
            return 3;
        else if (score >= currentLevel.twoStarScore)
            return 2;
        else if (score >= currentLevel.targetScore)
            return 1;
        else
            return 0;
    }

    public void OnNextLevelButtonClicked()
    {
        endPanel.SetActive(false);
        LevelManager.Instance.LoadNextLevel();
    }

    public void OnRestartLevelButtonClicked()
    {
        endPanel.SetActive(false);
        gameManager.RestartLevel();
    }

    public void OnExitToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}

