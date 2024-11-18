using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public float countdownTime = 3f; // Geri say�m s�resi (saniye)
    public TextMeshProUGUI countdownText; // Geri say�m metni

    private void Start()
    {
        // Oyunu duraklat
        Time.timeScale = 0f;

        // Geri say�m� ba�lat
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            // Metni g�ncelle
            countdownText.text = Mathf.CeilToInt(remainingTime).ToString();

            // Bir sonraki kareye kadar bekle
            yield return new WaitForSecondsRealtime(1f);

            remainingTime--;
        }

        // Geri say�m bitti�inde
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        // Metni gizle veya yok et
        countdownText.gameObject.SetActive(false);

        // Oyunu ba�lat
        Time.timeScale = 1f;
    }
}
