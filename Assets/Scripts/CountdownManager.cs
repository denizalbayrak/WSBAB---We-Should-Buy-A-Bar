using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public float countdownTime = 3f; // Geri sayým süresi (saniye)
    public TextMeshProUGUI countdownText; // Geri sayým metni

    private void Start()
    {
        // Oyunu duraklat
        Time.timeScale = 0f;

        // Geri sayýmý baþlat
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            // Metni güncelle
            countdownText.text = Mathf.CeilToInt(remainingTime).ToString();

            // Bir sonraki kareye kadar bekle
            yield return new WaitForSecondsRealtime(1f);

            remainingTime--;
        }

        // Geri sayým bittiðinde
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        // Metni gizle veya yok et
        countdownText.gameObject.SetActive(false);

        // Oyunu baþlat
        Time.timeScale = 1f;
    }
}
