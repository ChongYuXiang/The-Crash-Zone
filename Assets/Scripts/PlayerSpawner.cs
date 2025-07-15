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
    private GameObject CrashCourser1;
    [SerializeField]
    private GameObject CrashCourser2;
    [SerializeField]
    private GameObject WinchWrangler1;
    [SerializeField]
    private GameObject WinchWrangler2;
    [SerializeField]
    private GameObject SirenSaviour1;
    [SerializeField]
    private GameObject SirenSaviour2;
    [SerializeField]
    private GameObject RoaringRacer1;
    [SerializeField]
    private GameObject RoaringRacer2;

    public string player1CarID;
    public string player2CarID;

    public void SaveNFCID(string ID)
    {
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
        }
        if (playerNum == 2)
        {
            player2CarID = null;
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
        Debug.Log("Spawning P1...");
        if (player1CarID == "04-C4-9A-15-C2-2A-81")
        {
            // Spawn CrashCourser1 at spawn point 1
            GameObject clone = (GameObject)Instantiate(CrashCourser1, spawnPoint1.position, spawnPoint1.rotation);
            Debug.Log("Player 1 (CrashCourser) spawned");
        }
        if (player1CarID == "04-E1-7E-16-C2-2A-81")
        {
            // Spawn WinchWrangler1 at spawn point 1
            GameObject clone = (GameObject)Instantiate(WinchWrangler1, spawnPoint1.position, spawnPoint1.rotation);
            Debug.Log("Player 1 (WinchWrangler) spawned");
        }
        if (player1CarID == "04-1A-AD-15-C2-2A-81")
        {
            // Spawn SirenSaviour1 at spawn point 1
            GameObject clone = (GameObject)Instantiate(SirenSaviour1, spawnPoint1.position, spawnPoint1.rotation);
            Debug.Log("Player 1 (SirenSaviour) spawned");
        }
        if (player1CarID == "04-21-43-15-C2-2A-81")
        {
            // Spawn RoaringRacer1 at spawn point 1
            GameObject clone = (GameObject)Instantiate(RoaringRacer1, spawnPoint1.position, spawnPoint1.rotation);
            Debug.Log("Player 1 (RoaringRacer) spawned");
        }
    }

    private void SpawnPlayer2()
    {
        Debug.Log("Spawning P2...");
        if (player2CarID == "04-C4-9A-15-C2-2A-81")
        {
            // Spawn CrashCourser2 at spawn point 2
            GameObject clone = (GameObject)Instantiate(CrashCourser2, spawnPoint2.position, spawnPoint2.rotation);
            Debug.Log("Player 2 (CrashCourser) spawned");
        }
        if (player2CarID == "04-E1-7E-16-C2-2A-81")
        {
            // Spawn WinchWrangler2 at spawn point 2
            GameObject clone = (GameObject)Instantiate(WinchWrangler2, spawnPoint2.position, spawnPoint2.rotation);
            Debug.Log("Player 2 (WinchWrangler) spawned");
}
        if (player2CarID == "04-1A-AD-15-C2-2A-81")
        {
            // Spawn SirenSaviour2 at spawn point 2
            GameObject clone = (GameObject)Instantiate(SirenSaviour2, spawnPoint2.position, spawnPoint2.rotation);
            Debug.Log("Player 2 (SirenSaviour) spawned");
        }
        if (player1CarID == "04-21-43-15-C2-2A-81")
        {
            // Spawn RoaringRacer2 at spawn point 1
            GameObject clone = (GameObject)Instantiate(RoaringRacer2, spawnPoint2.position, spawnPoint2.rotation);
            Debug.Log("Player 2 (RoaringRacer) spawned");
        }
    }
}
