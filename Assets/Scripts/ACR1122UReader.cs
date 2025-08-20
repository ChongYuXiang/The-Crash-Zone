using UnityEngine;
using Lando;
using System;
using TMPro;
using System.Collections; // <-- The namespace from Lando.dll

public class ACR122UReader : MonoBehaviour
{
    private Cardreader cardReader;
    private int currentPlayer;

    public GameObject ScanningScreen;
    public Transform DisplayPos1;
    public Transform DisplayPos2;
    public TextMeshProUGUI p1Title;
    public TextMeshProUGUI p2Title;
    public GameObject CC_Display1;
    public GameObject CC_Display2;
    public GameObject WW_Display1;
    public GameObject WW_Display2;
    public GameObject SS_Display1;
    public GameObject SS_Display2;
    public GameObject RR_Display1;
    public GameObject RR_Display2;
    public GameObject MM_Display1;
    public GameObject MM_Display2;

    // Called when the script is first enabled
    private void Start()
    {
        // Instantiate the Lando card readern
        cardReader = new Cardreader();

        // Subscribe to the CardConnected event
        cardReader.CardConnected += OnCardConnected;
        // Subscribe to the CardDisconnected event
        cardReader.CardDisconnected += OnCardDisconnected;

        cardReader.StartWatch();
    }

    public void StartScanning(int playerNum)
    {
        // Start watching for NFC cards
        currentPlayer = playerNum;
        if (currentPlayer == 1) 
        {
            p1Title.text = "";
        }
        if (currentPlayer == 2)
        {
            p2Title.text = "";
        }
    }

   
    // This method is called every time a card is detected
    private void OnCardConnected(object sender, CardreaderEventArgs e)
    {
        // The e.Card.Id property contains the card UID
        string cardId = e.Card.Id;
        Debug.Log($"ACR122U: Card connected with UID: {cardId}");

        GameObject carToSpawn = null;
        if (currentPlayer == 1) // Spawns for player 1
        {
            if (cardId == "04-21-43-15-C2-2A-81") // ID for CrashCourser
            {
                GameManager.instance.SelectCar(1, "CrashCourser");
                ScanningScreen.SetActive(false);
                carToSpawn = CC_Display1;
                p1Title.text = "CRASH-COURSER";
            }
            if (cardId == "04-E1-7E-16-C2-2A-81") // ID for WinchWrangler
            {
                GameManager.instance.SelectCar(1, "WinchWrangler");
                ScanningScreen.SetActive(false);
                carToSpawn = WW_Display1;
                p1Title.text = "WINCH-WRANGLER";
            }
            if (cardId == "04-C4-9A-15-C2-2A-81") // ID for SirenSaviour
            {
                GameManager.instance.SelectCar(1, "SirenSaviour");
                ScanningScreen.SetActive(false);
                carToSpawn = SS_Display1;
                p1Title.text = "SIREN-SAVIOUR";
            }
            if (cardId == "04-1A-AD-15-C2-2A-81") // ID for RoaringRacer
            {
                GameManager.instance.SelectCar(1, "RoaringRacer");
                ScanningScreen.SetActive(false);
                carToSpawn = RR_Display1;
                p1Title.text = "ROARING-RACER";
            }
            if (cardId == "04-FD-1F-16-C2-2A-81") // ID for MasterMixer
            {
                GameManager.instance.SelectCar(1, "MasterMixer");
                ScanningScreen.SetActive(false);
                carToSpawn = MM_Display1;
                p1Title.text = "MASTER-MIXER";
            }

            Instantiate(carToSpawn, DisplayPos1, worldPositionStays: false); // Spawn the display car
        }

        if (currentPlayer == 2) // Spawns for player 2
        {
            if (cardId == "04-21-43-15-C2-2A-81") // ID for CrashCourser
            {
                GameManager.instance.SelectCar(2, "CrashCourser");
                ScanningScreen.SetActive(false);
                carToSpawn = CC_Display2;
                p2Title.text = "CRASH-COURSER";
            }
            if (cardId == "04-E1-7E-16-C2-2A-81") // ID for WinchWrangler
            {
                GameManager.instance.SelectCar(2, "WinchWrangler");
                ScanningScreen.SetActive(false);
                carToSpawn = WW_Display2;
                p2Title.text = "WINCH-WRANGLER";
            }
            if (cardId == "04-C4-9A-15-C2-2A-81") // ID for SirenSaviour
            {
                GameManager.instance.SelectCar(2, "SirenSaviour");
                ScanningScreen.SetActive(false);
                carToSpawn = SS_Display2;
                p2Title.text = "SIREN-SAVIOUR";
            }
            if (cardId == "04-1A-AD-15-C2-2A-81") // ID for RoaringRacer
            {
                GameManager.instance.SelectCar(2, "RoaringRacer");
                ScanningScreen.SetActive(false);
                carToSpawn = RR_Display2;
                p2Title.text = "ROARING-RACER";
            }
            if (cardId == "04-FD-1F-16-C2-2A-81") // ID for MasterMixer
            {
                GameManager.instance.SelectCar(2, "MasterMixer");
                ScanningScreen.SetActive(false);
                carToSpawn = MM_Display2;
                p2Title.text = "MASTER-MIXER";
            }

            Instantiate(carToSpawn, DisplayPos2, worldPositionStays: false); // Spawn the display car
        }
    }

    // This method is called every time a card is disconnected
    private void OnCardDisconnected(object sender, CardreaderEventArgs e)
    {
        Debug.Log("ACR122U: Card disconnected");
        currentPlayer = 0;
    }

    // Called when the script or the GameObject is destroyed or the scene changes
    private void OnDestroy()
    {
        // Stop watching for new cards
        cardReader.StopWatch();

        // Dispose to free resources
        cardReader.Dispose();
        Debug.Log("Lando: Stopped watching and disposed reader");

        // Unsubscribe from the events
        cardReader.CardConnected -= OnCardConnected;
        cardReader.CardDisconnected -= OnCardDisconnected;
    }


    public void SwitchButton(int playerNum)// For modded version without need for NFC
    {
        StartCoroutine(SwitchCar(playerNum));
    }

    IEnumerator SwitchCar(int playerNum)
    {
        GameObject carToSpawn = null;
        if (playerNum == 1)
        {
            if (GameManager.instance.p1Car == "CrashCourser")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(1, "WinchWrangler");
                GameManager.instance.SelectWrap(1, 0);
                carToSpawn = WW_Display1;
                p1Title.text = "WINCH-WRANGLER";
            }
            else if (GameManager.instance.p1Car == "WinchWrangler")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(1, "SirenSaviour");
                GameManager.instance.SelectWrap(1, 0);
                carToSpawn = SS_Display1;
                p1Title.text = "SIREN-SAVIOUR";
            }
            else if (GameManager.instance.p1Car == "SirenSaviour")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(1, "RoaringRacer");
                GameManager.instance.SelectWrap(1, 0);
                carToSpawn = RR_Display1;
                p1Title.text = "ROARING-RACER";
            }
            else if (GameManager.instance.p1Car == "RoaringRacer")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(1, "MasterMixer");
                GameManager.instance.SelectWrap(1, 0);
                carToSpawn = MM_Display1;
                p1Title.text = "MASTER-MIXER";
            }
            else if (GameManager.instance.p1Car == "MasterMixer")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(1, "CrashCourser");
                GameManager.instance.SelectWrap(1, 0);
                carToSpawn = CC_Display1;
                p1Title.text = "CRASH-COURSER";
            }
            Instantiate(carToSpawn, DisplayPos1, worldPositionStays: false); // Spawn the display car
        }
        if (playerNum == 2)
        {
            if (GameManager.instance.p2Car == "CrashCourser")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(2, "WinchWrangler");
                GameManager.instance.SelectWrap(2, 0);
                carToSpawn = WW_Display2;
                p2Title.text = "WINCH-WRANGLER";
            }
            else if (GameManager.instance.p2Car == "WinchWrangler")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(2, "SirenSaviour");
                GameManager.instance.SelectWrap(2, 0);
                carToSpawn = SS_Display2;
                p2Title.text = "SIREN-SAVIOUR";
            }
            else if (GameManager.instance.p2Car == "SirenSaviour")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(2, "RoaringRacer");
                GameManager.instance.SelectWrap(2, 0);
                carToSpawn = RR_Display2;
                p2Title.text = "ROARING-RACER";
            }
            else if (GameManager.instance.p2Car == "RoaringRacer")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(2, "MasterMixer");
                GameManager.instance.SelectWrap(2, 0);
                carToSpawn = MM_Display2;
                p2Title.text = "MASTER-MIXER";
            }
            else if (GameManager.instance.p2Car == "MasterMixer")
            {
                yield return new WaitForEndOfFrame();
                GameManager.instance.SelectCar(2, "CrashCourser");
                GameManager.instance.SelectWrap(2, 0);
                carToSpawn = CC_Display2;
                p2Title.text = "CRASH-COURSER";
            }
            Instantiate(carToSpawn, DisplayPos2, worldPositionStays: false); // Spawn the display car
        }
    }

}