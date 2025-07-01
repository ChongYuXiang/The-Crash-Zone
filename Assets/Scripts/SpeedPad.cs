using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            car.ApplyInstantSpeedBoost(50f, 1f); // 60 speed for 1 seconds
        }
    }

}
