using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cement : MonoBehaviour
{
    public float slowSpeed = 5f;
    public float slowAcceleration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null && car.gameObject.name != "MasterMixer")
        {
            //AudioManager.instance.PlaySFX("CementEnter");
            car.EnterSlowZone(slowSpeed, slowAcceleration);
            AudioManager.instance.PlaySFX("CementEnter");
        }
        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null)
        {
            AudioManager.instance.PlaySFX("CementEnter");
            enemy.EnterSlowZone(slowSpeed, slowAcceleration);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null && car.gameObject.name != "MasterMixer")
        {
            car.ExitSlowZone();
        }
        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null)
        {
            enemy.ExitSlowZone();
            return;
        }
    }
}
