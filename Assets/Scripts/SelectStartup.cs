using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectStartup : MonoBehaviour
{
    public TextMeshProUGUI gameModeTitle;
    public GameObject player2UI;
    public GameObject enemyUI;
    public GameObject player2Car;
    public GameObject enemyCar;
    public GameObject arenaMenu;
    public GameObject trackMenu;
    public JumpToUIElement nextButton;
    public Selectable ArenaButton;
    public Selectable RaceButton;

    private void Start()
    {
        if (GameManager.instance.gameMode == "Arena")
        {
            if (GameManager.instance.playerCount == "VS")
            {
                gameModeTitle.text = "ARENA VS";
            }
            if (GameManager.instance.playerCount == "Solo")
            {
                gameModeTitle.text = "ARENA SOLO";
                player2UI.SetActive(false);
                player2Car.SetActive(false);
                enemyUI.SetActive(true);
                enemyCar.SetActive(true);
            }
            arenaMenu.SetActive(true);
            nextButton.elementToSelect = ArenaButton;
        }
        if (GameManager.instance.gameMode == "Racing")
        {
            if (GameManager.instance.playerCount == "VS")
            {
                gameModeTitle.text = "RACING VS";
            }
            if (GameManager.instance.playerCount == "Solo")
            {
                gameModeTitle.text = "RACING SOLO";
                player2UI.SetActive(false);
                player2Car.SetActive(false);
            }
            trackMenu.SetActive(true);
            nextButton.elementToSelect = RaceButton;
        }
    }
}
