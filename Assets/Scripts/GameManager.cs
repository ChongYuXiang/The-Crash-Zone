using System;
using System.Collections;
using System.Collections.Generic;
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
        if (selectedMap == "TestZone")
        {
            SceneController.instance.LoadScene(2);
        }
    }
}
