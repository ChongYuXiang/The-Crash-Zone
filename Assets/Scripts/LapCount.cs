using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapCount : MonoBehaviour
{
    private int lapcountP1 = 0;
    private int lapcountP2 = 0;

    public TextMeshProUGUI lapText1;
    public TextMeshProUGUI lapText2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player1Colliders")
        {
            lapcountP1 += 1;
            lapText1.text = lapcountP1.ToString() + "/3";
            Debug.Log("P1 + 1");
            CheckWinner();
        }
        if (other.gameObject.tag == "player2Colliders")
        {
            lapcountP2 += 1;
            lapText2.text = lapcountP2.ToString() + "/3";
            Debug.Log("P2 + 1");
            CheckWinner();
        }
    }

    public void CheckWinner()
    {
        if (lapcountP1 >= 3 && lapcountP2 <= lapcountP1)
        {
            Debug.Log("P1 wins");
        }
        else if (lapcountP2 >= 3 && lapcountP1 <= lapcountP2)
        {
            Debug.Log("P2 wins");
        }
    }
}
