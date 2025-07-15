using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPad : MonoBehaviour
{
    public float launchForce = 1000f; // Adjustable in Inspector

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the spring pad: " + other.name);
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Optional: only apply to cars with PlayersCarController
            PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
            if (car != null)
            {
                // Apply upward force
                rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
                Debug.Log("Launched car upward!");
            }
        }
    }
}
