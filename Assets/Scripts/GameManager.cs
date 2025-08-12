using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string gameMode;
    public string playerCount;

    public string p1Car;
    public string p2Car;

    public int p1Wrap;
    public int p2Wrap;

    public string selectedMap;

    public TextMeshProUGUI winnerText;
    public GameObject victoryScreen;
    public bool gameOver = false;

    public GameObject sparks;
    public GameObject explosion;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SelectGameMode(string mode, string players)
    {
        gameMode = mode;
        playerCount = players;
    }

    public void SelectCar(int player, string carName)
    {
        if (player == 1)
        {
            p1Car = carName;
        }
        if (player == 2)
        {
            p2Car = carName;
        }
    }

    public void SelectWrap(int player, int wrapIndex)
    {
        if (player == 1)
        {
            p1Wrap = wrapIndex;
        }
        if (player == 2)
        {
            p2Wrap = wrapIndex;
        }
    }

    public void ToScene()
    {
        if (selectedMap == "CrashZone")
        {
            SceneController.instance.LoadScene(2);
        }
        if (selectedMap == "ConstructionZone")
        {
            SceneController.instance.LoadScene(3);
        }
        if (selectedMap == "FroZone")
        {
            SceneController.instance.LoadScene(4);
        }
        if (selectedMap == "TestZone")
        {
            SceneController.instance.LoadScene(5);
        }
        if (selectedMap == "AutumnTrack")
        {
            SceneController.instance.LoadScene(6);
        }
        if (selectedMap == "SummerTrack")
        {
            SceneController.instance.LoadScene(7);
        }
    }

    public void PlayerWins(int loserIndex)
    {
        if (!gameOver)
        {
            victoryScreen.SetActive(true);
            if (loserIndex == 1)
            {
                winnerText.text = "PLAYER 2 WINS!";
            }
            if (loserIndex == 2)
            {
                winnerText.text = "PLAYER 1 WINS!";
            }
            if (loserIndex == 0)
            {
                winnerText.text = "TIME OUT: TIE!";
            }
            gameOver = true;

            AudioManager.instance.PlayBGM("Quiet");
        }
    }

    public IEnumerator playerVFX(int pIndex, string VFX)
    {
        GameObject targetPos = null;
        if (pIndex == 1)
        {
            targetPos = GameObject.Find("player1Target");
        }
        if (pIndex == 2)
        {
            targetPos = GameObject.Find("player2Target");
        }
        if (VFX == "sparks")
        {
            GameObject clone = (GameObject)Instantiate(sparks, targetPos.transform.position, targetPos.transform.rotation);
            yield return new WaitForSeconds(1);
            Destroy(clone);
        }
        if (VFX == "explosion")
        {
            GameObject clone = (GameObject)Instantiate(explosion, targetPos.transform.position, targetPos.transform.rotation);
            yield return new WaitForSeconds(3);
            Destroy(clone);
        }
    }
}
