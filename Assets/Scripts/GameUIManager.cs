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
                                             // Yeni eklenen de�i�kenler
    public float countdownTime = 3f; // Geri say�m s�resi
    public TextMeshProUGUI countdownText; // Geri say�m metni referans�
    public GameObject pauseMenuUI; // Pause UI referans�
    public TextMeshProUGUI levelTimerText; // Level s�resi i�in metin
    public Slider levelTimerSlider; // Level s�resi i�in slider

    private float levelDuration; // Level s�resi
    private float timeRemaining; // Kalan s�re
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
    // Pause fonksiyonlar�
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
        SceneManager.LoadScene("MainMenuScene"); // Ana men� sahnenizin ad�n� yaz�n
    }
    public void StartLevelTimer(float duration)
    {
        levelDuration = duration;
        timeRemaining = duration;

        // Slider ayarlar�
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
            // Metni g�ncelle
            if (countdownText != null)
            {
                countdownText.text = Mathf.CeilToInt(remainingTime).ToString();
            }

            // Bir sonraki kareye kadar bekle
            yield return new WaitForSecondsRealtime(1f);

            remainingTime--;
        }

        // Geri say�m bitti�inde
        if (countdownText != null)
        {
            countdownText.text = "GO!";
        }
        yield return new WaitForSecondsRealtime(1f);

        // Oyunu ba�lat
        Time.timeScale = 1f;
        countdownText.transform.parent.gameObject.SetActive(false);
        countdownText.transform.parent.parent.gameObject.SetActive(false);
        gameManager.currentGameState = GameState.InGame;

        // Level s�resini ba�lat
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

            // Metni g�ncelle
            if (levelTimerText != null)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);
                levelTimerText.text = $"{minutes:00}:{seconds:00}";
            }

            // Slider'� g�ncelle
            if (levelTimerSlider != null)
            {
                levelTimerSlider.value = timeRemaining;
            }

            yield return null;
        }

        // S�re doldu�unda bir �ey yap
        if (levelTimerText != null)
        {
            levelTimerText.text = "00:00";
        }

        Debug.Log("Level time is over!");
    }
}

