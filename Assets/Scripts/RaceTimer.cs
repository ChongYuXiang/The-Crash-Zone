using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceTimer : MonoBehaviour
{
    public static RaceTimer instance;

    public TextMeshProUGUI timerText;
    private float raceTime = 0f;
    private bool isRunning = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!isRunning) return;

        raceTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(raceTime / 60f);
        int seconds = Mathf.FloorToInt(raceTime % 60f);
        int milliseconds = Mathf.FloorToInt((raceTime * 1000f) % 1000f);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    public void StartTimer()
    {
        raceTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetRaceTime()
    {
        return raceTime;
    }
}