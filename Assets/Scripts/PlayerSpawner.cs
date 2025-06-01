using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint1;
    [SerializeField] 
    private Transform spawnPoint2;

    [SerializeField] 
    private GameObject defaultCar1;
    [SerializeField]
    private GameObject defaultCar2;

    public string player1CarID;
    public string player2CarID;

    public void SaveNFCID(string ID)
    {
        Debug.Log("Saving... ID sent: " + ID);

        if (string.IsNullOrEmpty(player1CarID))
        {
            // Save player 1's car ID
            player1CarID = ID;
            Debug.Log("Player 1 saved as " +  ID);
            SpawnPlayer1();
        }
        else if (string.IsNullOrEmpty(player2CarID))
        {
            // Save player 2's car ID
            player2CarID = ID;
            Debug.Log("Player 2 saved as " + ID);
            SpawnPlayer2();
        }
    }

    public void RemovePlayer(int playerNum)
    {
        if (playerNum == 1)
        {
            player1CarID = null;
            // Add a script here to delete clone1
        }
        if (playerNum == 2)
        {
            player2CarID = null;
            // Add a script here to delete clone2
        }
    } 

    public void StartGame()
    {
        if (player1CarID == null || player2CarID == null)
        {
            Debug.Log("IDs not set");
            return;
        }

        SpawnPlayer1();
        SpawnPlayer2();
    }

    private void SpawnPlayer1()
    {
        if (player1CarID == "04-C4-9A-15-C2-2A-81")
        {
            // Spawn player 1 at spawn point 1
            GameObject clone1 = (GameObject)Instantiate(defaultCar1, spawnPoint1.position, spawnPoint1.rotation);
            Debug.Log("Player 1 spawned");
        }
    }

    private void SpawnPlayer2() 
    {
        if (player2CarID == "04-E1-7E-16-C2-2A-81")
        {
            // Spawn player 2 at spawn point 2
            GameObject clone2 = (GameObject)Instantiate(defaultCar2, spawnPoint2.position, spawnPoint2.rotation);
            Debug.Log("Player 2 spawned");
        }
    }
}
