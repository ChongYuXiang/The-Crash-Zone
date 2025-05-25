using UnityEngine;
using Lando; // <-- The namespace from Lando.dll

public class ACR122UReader : MonoBehaviour
{
    private Cardreader cardReader;

    [SerializeField]
    private GameObject playerSpawner;

    // Called when the script is first enabled
    private void Start()
    {
        // Instantiate the Lando card reader
        cardReader = new Cardreader();

        // Subscribe to the CardConnected event
        cardReader.CardConnected += OnCardConnected;
        // Subscribe to the CardDisconnected event
        cardReader.CardDisconnected += OnCardDisconnected;

        // Start watching for NFC cards
        cardReader.StartWatch();
        Debug.Log("Lando: Started watching for cards");
    }

    // This method is called every time a card is detected
    private void OnCardConnected(object sender, CardreaderEventArgs e)
    {
        // The e.Card.Id property contains the card UID
        string cardId = e.Card.Id;
        Debug.Log($"ACR122U: Card connected with UID: {cardId}");
        playerSpawner.GetComponent<PlayerSpawner>().SaveNFCID(cardId);
    }

    // This method is called every time a card is disconnected
    private void OnCardDisconnected(object sender, CardreaderEventArgs e)
    {
        Debug.Log("ACR122U: Card disconnected");
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
}