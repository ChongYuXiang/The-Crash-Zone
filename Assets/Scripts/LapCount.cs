using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCount : MonoBehaviour
{
    public int lapcountP1 = 0;
    public int lapcountP2 = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player1Colliders")
        {
            lapcountP1 += 1;
            Debug.Log("P1 + 1");
            CheckWinner();
        }
        if (other.gameObject.tag == "player2Colliders")
        {
            lapcountP2 += 1;
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
