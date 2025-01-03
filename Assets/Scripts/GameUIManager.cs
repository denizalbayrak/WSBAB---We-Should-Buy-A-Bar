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

    public GameObject WarningPanel;
    public TextMeshProUGUI WarningPanelText; 

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
    public GameObject[] tutorialList;
    public GameObject tutorialPanel;
    public GameObject finishPanel;

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
        SceneManager.LoadScene("MainMenuScene"); 
    }
    public void StartLevelTimer(float duration)
    {
        levelDuration = duration;
        timeRemaining = duration;


        if (levelTimerSlider != null)
        {
            levelTimerSlider.maxValue = duration;
            levelTimerSlider.value = duration;
        }

        StartCoroutine(UpdateLevelTimer());
    }

    private IEnumerator Countdown()
    {
    
        Debug.Log("countdown started");
        Time.timeScale = 0f;
        countdownText.transform.parent.parent.gameObject.SetActive(true);
        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
      
            if (countdownText != null)
            {
                countdownText.text = Mathf.CeilToInt(remainingTime).ToString();
            }

            yield return new WaitForSecondsRealtime(1f);

            remainingTime--;
        }


        if (countdownText != null)
        {
            countdownText.text = "GO!";
        }
        yield return new WaitForSecondsRealtime(1f);


        Time.timeScale = 1f;
        countdownText.transform.parent.parent.gameObject.SetActive(false);
        gameManager.currentGameState = GameState.InGame;

        StartLevelTimer(LevelManager.Instance.currentLevel.levelDuration);
        Debug.Log("GameStarted!");
    }
    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }
    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        foreach (var item in tutorialList)
        {
            item.SetActive(false);
        }
        if (LevelManager.Instance.currentLevelIndex < 5)
        {
            tutorialList[LevelManager.Instance.currentLevelIndex].SetActive(true);
        }
    }
    private IEnumerator UpdateLevelTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (levelTimerText != null)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);
                levelTimerText.text = $"{minutes:00}:{seconds:00}";
            }

            if (levelTimerSlider != null)
            {
                levelTimerSlider.value = timeRemaining;
            }

            yield return null;
        }

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

        int starCount = CalculateStarCount(score, currentLevel);

        for (int i = 0; i < stars.Length; i++)
        {
            if (i == 0)
            {
                stars[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentLevel.scoreFor1Star.ToString();
            }
            if (i == 1)
            {
                stars[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentLevel.scoreFor2Stars.ToString();
            }
            if (i == 2)
            {
                stars[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentLevel.scoreFor3Stars.ToString();
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

        if (starCount > 0)
        {
            LevelManager.Instance.CompleteLevel();
            if (LevelManager.Instance.currentLevelIndex == 9)
            {
                finishPanel.SetActive(true);
            }
            else
            {
                successPanel.SetActive(true);

            }
            failurePanel.SetActive(false);

            if (nextLevelButton != null)
            {
                nextLevelButton.interactable = true;
            }
        }
        else
        {
            successPanel.SetActive(false);
            failurePanel.SetActive(true);

            if (nextLevelButton != null)
            {
                nextLevelButton.interactable = false;
            }
        }
    }
    private int CalculateStarCount(int score, Level currentLevel)
    {
        if (score >= currentLevel.scoreFor3Stars)
            return 3;
        else if (score >= currentLevel.scoreFor2Stars)
            return 2;
        else if (score >= currentLevel.scoreFor1Star)
            return 1;
        else
            return 0;
    }
    public void SceneChange()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void OnNextLevelButtonClicked()
    {
        endPanel.SetActive(false);
        LevelManager.Instance.UnloadCurrentLevel();
        GameManager.Instance.DestroyPlayerCharacter();
        LevelManager.Instance.LoadNextLevel();
        GameManager.Instance.SpawnPlayerCharacter();
        GameManager.Instance.currentGameState = GameState.InGame;
        GameUIManager.Instance.StartCountdown();
    }

    public void OnRestartLevelButtonClicked()
    {
        endPanel.SetActive(false);
        gameManager.RestartLevel();
    }

    public void OnExitToMainMenuButtonClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.currentGameState = GameState.InGame;
        GameManager.Instance.selectedLevelIndex = 0;
        GameManager.Instance.currentSaveData = null;
        GameManager.Instance.DestroyPlayerCharacter();
        LevelManager.Instance.UnloadCurrentLevel();
        SceneManager.LoadScene("MainMenuScene");
    }
}

