using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleLC : MonoBehaviour
{
    public int totalLaps = 3;
    private int currentLap = 0;
    private bool hasFinished = false;

    public TextMeshProUGUI lapText; // Assign in inspector
    public float lapCooldown = 3f;  // Cooldown to prevent repeated triggers
    private float lastLapTime = -999f;

    void Start()
    {
        UpdateLapUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LapTrigger") || hasFinished)
            return;

        // Check cooldown
        if (Time.time - lastLapTime < lapCooldown)
            return;

        lastLapTime = Time.time;

        // First pass just starts the race, don't count as a lap
        if (currentLap == 0)
        {
            Debug.Log("Race started!");
        }
        else
        {
            Debug.Log($"Lap {currentLap}/{totalLaps}");
        }

        currentLap++;

        UpdateLapUI();

        if (currentLap > totalLaps)
        {
            hasFinished = true;

            if (RaceTimer.instance != null)
                RaceTimer.instance.StopTimer();

            Debug.Log("You finished the race!");
        }
    }

    private void UpdateLapUI()
    {
        int displayLap = Mathf.Clamp(currentLap, 0, totalLaps);
        if (lapText != null)
            lapText.text = $"Lap: {displayLap}/{totalLaps}";
    }
}
