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
            car.EnterSlowZone(slowSpeed, slowAcceleration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            car.ExitSlowZone();
        }
    }

    /* If want add a cooldown for the slowpad
    public class SlowPad : MonoBehaviour
{
    public float slowSpeed = 5f;
    public float slowAcceleration = 5f;
    public float cooldownDuration = 2f;

    private Dictionary<PlayersCarController, float> carCooldowns = new Dictionary<PlayersCarController, float>();

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            // Check if car is still in cooldown
            if (carCooldowns.TryGetValue(car, out float lastExitTime))
            {
                if (Time.time - lastExitTime < cooldownDuration)
                {
                    Debug.Log("Car still in cooldown, skipping slow effect.");
                    return;
                }
            }

            // Apply slow effect
            car.EnterSlowZone(slowSpeed, slowAcceleration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            car.ExitSlowZone();

            // Record cooldown start time
            if (carCooldowns.ContainsKey(car))
                carCooldowns[car] = Time.time;
            else
                carCooldowns.Add(car, Time.time);
        }
    }
} 
     */
}
