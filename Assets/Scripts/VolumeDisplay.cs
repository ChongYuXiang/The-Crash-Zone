using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolumeDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    public void DisplayVolume(float value)
    {
        displayText.text = Mathf.Round(value * 100).ToString() + "%";
    }
}
