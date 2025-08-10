using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeDisplay : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI displayText;
    public bool isBGM;

    private void Update()
    {
        if (isBGM)
        {
            displayText.text = Mathf.Round(AudioManager.instance.BGMSource.volume * 100).ToString() + "%";
        }
        else
        {
            displayText.text = Mathf.Round(AudioManager.instance.SFXSource.volume * 100).ToString() + "%";
        }
    }

    private void Start()
    {
        if (isBGM)
        {
            displayText.text = Mathf.Round(AudioManager.instance.BGMSource.volume * 100).ToString() + "%";
            volumeSlider.value = Mathf.Round(AudioManager.instance.BGMSource.volume*10)/10;
        }
        else
        {
            displayText.text = Mathf.Round(AudioManager.instance.SFXSource.volume * 100).ToString() + "%";
            volumeSlider.value = Mathf.Round(AudioManager.instance.SFXSource.volume*10)/10;
        }
    }
}
