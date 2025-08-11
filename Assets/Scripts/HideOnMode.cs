using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnMode : MonoBehaviour
{
    public string modeToHide;

    private void Start()
    {
        if (GameManager.instance.playerCount == modeToHide)
        {
            gameObject.SetActive(false);
        }
    }
}
