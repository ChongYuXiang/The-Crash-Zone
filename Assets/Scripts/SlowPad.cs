using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPad : MonoBehaviour
{
    public float slowSpeed = 5f;
    public float slowAcceleration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            AudioManager.instance.PlaySFX("SlowPad");
            car.EnterSlowZone(slowSpeed, slowAcceleration);
            return;
        }
        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null)
        {
            AudioManager.instance.PlaySFX("SlowPad");
            enemy.EnterSlowZone(slowSpeed, slowAcceleration);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            car.ExitSlowZone();
            return;
        }
        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null)
        {
            enemy.ExitSlowZone();
            return;
        }
    }
}
