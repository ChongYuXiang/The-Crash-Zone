using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{
    public int playerNum;
    public Image healthStat;
    public Image SpeedStat;
    public TextMeshProUGUI abilityText;

    public void UpdateStats()
    {
        if (playerNum == 1)
        {
            if (GameManager.instance.p1Car == "CrashCourser")
            {
                healthStat.fillAmount = 1;
                SpeedStat.fillAmount = 1;
                abilityText.text = "None";
            }
            if (GameManager.instance.p1Car == "WinchWrangler")
            {
                healthStat.fillAmount = 0.66f;
                SpeedStat.fillAmount = 0.66f;
                abilityText.text = "Wrangling Hook";
            }
            if (GameManager.instance.p1Car == "SirenSaviour")
            {
                healthStat.fillAmount = 1;
                SpeedStat.fillAmount = 0.33f;
                abilityText.text = "Healing Field";
            }
            if (GameManager.instance.p1Car == "RoaringRacer")
            {
                healthStat.fillAmount = 0.33f;
                SpeedStat.fillAmount = 1;
                abilityText.text = "Super Boost";
            }
            if (GameManager.instance.p1Car == "MasterMixer")
            {
                healthStat.fillAmount = 1f;
                SpeedStat.fillAmount = 0.33f;
                abilityText.text = "Cement Pour";
            }
        }
        if (playerNum == 2)
        {
            if (GameManager.instance.p2Car == "CrashCourser")
            {
                healthStat.fillAmount = 1;
                SpeedStat.fillAmount = 1;
                abilityText.text = "None";
            }
            if (GameManager.instance.p2Car == "WinchWrangler")
            {
                healthStat.fillAmount = 0.66f;
                SpeedStat.fillAmount = 0.66f;
                abilityText.text = "Wrangling Hook";
            }
            if (GameManager.instance.p2Car == "SirenSaviour")
            {
                healthStat.fillAmount = 1;
                SpeedStat.fillAmount = 0.33f;
                abilityText.text = "Healing Field";
            }
            if (GameManager.instance.p2Car == "RoaringRacer")
            {
                healthStat.fillAmount = 0.33f;
                SpeedStat.fillAmount = 1;
                abilityText.text = "Super Boost";
            }
            if (GameManager.instance.p2Car == "MasterMixer")
            {
                healthStat.fillAmount = 1f;
                SpeedStat.fillAmount = 0.33f;
                abilityText.text = "Cement Pour";
            }
        }
    }
}
