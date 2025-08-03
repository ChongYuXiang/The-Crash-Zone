using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestFreezeScript : MonoBehaviour
{
    [Header("Scene Restriction")]
    public string allowedSceneName = "BattleScene"; // Replace with your actual scene name

    [Header("Freeze Settings")]
    public float freezeDuration = 3f;
    public float checkInterval = 15f;

    private List<PlayersCarController> allPlayerCars = new List<PlayersCarController>();

    void Start()
    {
        // Ensure script only works in the allowed scene
        if (SceneManager.GetActiveScene().name != allowedSceneName)
        {
            Debug.Log("FreezeController disabled (wrong scene)");
            enabled = false;
            return;
        }

        // Find all players tagged as "Player"
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        if (playerObjects.Length == 0)
        {
            Debug.LogWarning("FreezeController: No GameObjects with tag 'Player' found.");
            enabled = false;
            return;
        }

        // Get their PlayersCarController scripts
        foreach (GameObject playerObj in playerObjects)
        {
            PlayersCarController controller = playerObj.GetComponent<PlayersCarController>();
            if (controller != null)
            {
                allPlayerCars.Add(controller);
            }
        }

        if (allPlayerCars.Count == 0)
        {
            Debug.LogWarning("FreezeController: No PlayersCarController components found on players.");
            enabled = false;
            return;
        }

        // Start checking for random freezes
        StartCoroutine(FreezeCheckerLoop());
    }

    IEnumerator FreezeCheckerLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            float roll = Random.value;

            if (roll < 0.2f)
            {
                // 20% chance: Freeze both players
                Debug.Log("Freezing BOTH players");
                foreach (var player in allPlayerCars)
                {
                    StartCoroutine(FreezePlayer(player));
                }
            }
            else if (roll < 0.7f)
            {
                // 50% chance (0.2 to 0.7): Freeze a single random player
                int index = Random.Range(0, allPlayerCars.Count);
                Debug.Log($"Freezing ONE player: {allPlayerCars[index].gameObject.name}");
                StartCoroutine(FreezePlayer(allPlayerCars[index]));
            }
            else
            {
                // 30% chance (0.7 to 1): Freeze no one this interval
                Debug.Log("No freeze this time");
            }
        }
    }

    IEnumerator FreezePlayer(PlayersCarController playerCar)
    {
        Debug.Log($"Freezing player: {playerCar.gameObject.name}");
        playerCar.isFrozen = true;

        yield return new WaitForSeconds(freezeDuration);

        playerCar.isFrozen = false;
        Debug.Log($"Unfrozen player: {playerCar.gameObject.name}");
    }
}