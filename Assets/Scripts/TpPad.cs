using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpPad : MonoBehaviour
{
    public Transform destinationPad;

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null && car.CanTeleport())
        {
            Teleport(car);
            car.RegisterTeleport();
        }
    }

    void Teleport(PlayersCarController car)
    {
        Rigidbody rb = car.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = destinationPad.position + Vector3.up * 1f;
            rb.velocity = Vector3.zero;
            Debug.Log($"Teleported {car.name} to {destinationPad.name}");
        }
    }
}
