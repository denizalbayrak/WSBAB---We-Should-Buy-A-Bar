using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public float countdownTime = 3f; 
    public TextMeshProUGUI countdownText; 

    private void Start()
    {
        Time.timeScale = 0f;

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float remainingTime = countdownTime;
        while (remainingTime > 0)
        {
            countdownText.text = Mathf.CeilToInt(remainingTime).ToString();

            yield return new WaitForSecondsRealtime(1f);

            remainingTime--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }
}
