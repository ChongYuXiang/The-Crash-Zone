using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaTimer : MonoBehaviour
{
    public int countdownValue;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI[] pCounts;
    private PlayersCarController[] allCars;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(0.05f);
        allCars = FindObjectsOfType<PlayersCarController>(); // Find cars
        // Freeze all cars
        foreach (var car in allCars)
        {
            car.isFrozen = true;
        }

        yield return new WaitForSeconds(0.5f);
        foreach (TextMeshProUGUI text in pCounts)
        {
            text.text = "3";
            AudioManager.instance.PlaySFX("Count");
        }
        yield return new WaitForSeconds(1);
        foreach (TextMeshProUGUI text in pCounts)
        {
            text.text = "2";
            AudioManager.instance.PlaySFX("Count");
        }
        yield return new WaitForSeconds(1);
        foreach (TextMeshProUGUI text in pCounts)
        {
            text.text = "1";
            AudioManager.instance.PlaySFX("Count");
        }
        yield return new WaitForSeconds(1);
        foreach (TextMeshProUGUI text in pCounts)
        {
            text.text = "CRASH";
            AudioManager.instance.PlaySFX("Start");
        }
        yield return new WaitForSeconds(0.2f);

        // Unfreeze all cars
        foreach (var car in allCars)
        {
            car.isFrozen = false;
        }

        yield return new WaitForSeconds(0.2f);
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            AudioManager.instance.PlayBGM("CrashZone");
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            AudioManager.instance.PlayBGM("Construction");
        }
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            AudioManager.instance.PlayBGM("FroZone");
        }
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            AudioManager.instance.PlayBGM("Test");
        }

        yield return new WaitForSeconds(0.8f);
        foreach (TextMeshProUGUI text in pCounts)
        {
            text.text = "";
        }

        while (countdownValue > 0)
        {
            yield return new WaitForSeconds(1);
            countdownValue--;
            topText.text = countdownValue.ToString();
        }
        GameManager.instance.PlayerWins(0);
    }
}
