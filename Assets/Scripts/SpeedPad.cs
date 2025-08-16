using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedPad : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float duration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            AudioManager.instance.PlaySFX("BoostPad");
            car.ApplyInstantSpeedBoost(speed, duration);
            return;
        }
        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null)
        {
            AudioManager.instance.PlaySFX("BoostPad");
            enemy.ApplyInstantSpeedBoost(speed, duration);
            return;
        }
    }

}
