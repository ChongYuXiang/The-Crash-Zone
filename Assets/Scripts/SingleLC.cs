using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class SingleLC : MonoBehaviour
{
    [Header("Firebase Config")]
    public string gameMode = "single";    // e.g., "single", "versus", "timeTrial"
    public string arenaName = "arena_1";  // e.g., "desert", "city", "space"

    private bool hasSubmitted = false;

    private Firebase firebase;

    public int totalLaps = 3;
    private int currentLap = 0;
    private bool hasFinished = false;

    public TextMeshProUGUI lapText;
    public float lapCooldown = 10f;
    private float lastLapTime = -999f;

    public GameObject winPanel;
    public TMP_InputField nicknameInputField;
    public Button submitButton;

    void Start()
    {
        UpdateLapUI();
        if (winPanel != null)
            winPanel.SetActive(false);

        firebase = FindObjectOfType<Firebase>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFinished)
        {
            return;
        }
        // Cooldown check
        if (Time.time - lastLapTime < lapCooldown)
        {
            return;
        }

        lastLapTime = Time.time;
        currentLap++;

        UpdateLapUI();

        if (currentLap > totalLaps)
        {
            hasFinished = true;

            // Freeze this car
            var playerCar = other.GetComponent<PlayersCarController>();
            if (playerCar != null)
                playerCar.isFrozen = true;

            if (RaceTimer.instance != null)
                RaceTimer.instance.StopTimer();

            ShowWinUI();
        }
    }

    private void UpdateLapUI()
    {
        int displayLap = Mathf.Clamp(currentLap, 0, totalLaps);
        if (lapText != null)
            lapText.text = $"{displayLap}/{totalLaps}";
    }

    private void ShowWinUI()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

            StartCoroutine(FocusInput());

        if (RaceTimer.instance != null)
        {
            string time = RaceTimer.instance.GetCurrentTime();
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

    private IEnumerator FocusInput()
    {
        yield return new WaitForEndOfFrame();
        nicknameInputField.Select();
        nicknameInputField.ActivateInputField();
    }

    public void SubmitScore()
    {
        if (hasSubmitted) return;  // prevent duplicate submission
        hasSubmitted = true;

        string nickname = nicknameInputField.text.ToUpper();

        if (nickname.Length != 3 || !IsAlpha(nickname))
        {
            Debug.LogWarning("Nickname must be exactly 3 letters.");
            hasSubmitted = false;  // allow retry
            return;
        }

        float finalTime = RaceTimer.instance.GetRaceTime();

        Debug.Log($"Submitted Nickname: {nickname}, Time: {finalTime}");

        if (firebase != null)
        {
            firebase.SubmitScore(nickname, finalTime, gameMode, arenaName);
        }
        else
        {
            Debug.LogError("Firebase object not found in scene.");
        }

        submitButton.interactable = false;
        nicknameInputField.interactable = false;
        Debug.Log("SubmitScore() triggered");

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