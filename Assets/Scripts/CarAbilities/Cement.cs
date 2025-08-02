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
        if (car.gameObject.name != "MasterMixer")
        {
            //AudioManager.instance.PlaySFX("CementEnter");
            car.EnterSlowZone(slowSpeed, slowAcceleration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car.gameObject.name != "MasterMixer")
        {
            car.ExitSlowZone();
        }
    }
}
