using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        }
        yield return new WaitForSeconds(1);
        foreach (TextMeshProUGUI text in pCounts)
        {
            text.text = "2";
        }
        yield return new WaitForSeconds(1);
        foreach (TextMeshProUGUI text in pCounts)
        {
            text.text = "1";
        }
        yield return new WaitForSeconds(1);
        foreach (TextMeshProUGUI text in pCounts)
        {
            text.text = "CRASH";
        }
        yield return new WaitForSeconds(0.2f);

        // Unfreeze all cars
        foreach (var car in allCars)
        {
            car.isFrozen = false;
        }

        yield return new WaitForSeconds(1);
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
