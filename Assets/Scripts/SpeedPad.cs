using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedPad : MonoBehaviour
{
    public float boostSpeed = 60f;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the pad: " + other.name);

        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            Debug.Log("Car found! Applying speed boost...");
            car.ApplyInstantSpeedBoost(boostSpeed);
        }


    }
}
