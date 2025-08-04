using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SingleLC : MonoBehaviour
{
    public int totalLaps = 3;
    private int currentLap = 0;
    private bool hasFinished = false;

    public TextMeshProUGUI lapText; // Assign in inspector
    public float lapCooldown = 3f;  // Cooldown to prevent repeated triggers
    private float lastLapTime = -999f;

    public GameObject winPanel;
    public TextMeshProUGUI finalTimeText;
    public TMP_InputField nicknameInputField;
    public Button submitButton;
    // Optional leaderboard manager to send data to
    // public GameObject leaderboardManager;

    void Start()
    {
        UpdateLapUI();
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerCar") || hasFinished)
            return;

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

            ShowWinUI();
        }
    }

    private void UpdateLapUI()
    {
        int displayLap = Mathf.Clamp(currentLap, 0, totalLaps);
        if (lapText != null)
            lapText.text = $"Lap: {displayLap}/{totalLaps}";
    }

    private void ShowWinUI()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        if (finalTimeText != null && RaceTimer.instance != null)
        {
            string time = RaceTimer.instance.GetCurrentTime();
            finalTimeText.text = $"Final Time: {time}";
        }

        if (nicknameInputField != null)
        {
            nicknameInputField.characterLimit = 3;
            nicknameInputField.text = "";
            nicknameInputField.interactable = true;
        }

        if (submitButton != null)
        {
            submitButton.interactable = true;
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(SubmitScore);
        }

        Debug.Log("You won! Awaiting nickname input...");
    }

    public void SubmitScore()
    {
        string nickname = nicknameInputField.text.ToUpper();

        if (nickname.Length != 3 || !IsAlpha(nickname))
        {
            Debug.LogWarning("Nickname must be exactly 3 letters.");
            return;
        }

        float finalTime = RaceTimer.instance.GetRaceTime();

        Debug.Log($"Submitted Nickname: {nickname}, Time: {finalTime}");

        // TODO: Send this data to your leaderboard system
        // leaderboardManager.GetComponent<Leaderboard>().AddScore(nickname, finalTime);

        // Disable input after submission
        submitButton.interactable = false;
        nicknameInputField.interactable = false;
    }

    private bool IsAlpha(string input)
    {
        foreach (char c in input)
        {
            if (!char.IsLetter(c))
                return false;
        }
        return true;
    }
}