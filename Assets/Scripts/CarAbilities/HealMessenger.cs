using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMessenger : MonoBehaviour
{
    private HashSet<PlayersCarController> healingTargets = new HashSet<PlayersCarController>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player2Colliders") || other.CompareTag("player1Colliders"))
        {
            PlayersCarController player = other.GetComponentInParent<PlayersCarController>();
            if (player != null)
            {
                player.HealingOverTime(true);
                healingTargets.Add(player);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player2Colliders") || other.CompareTag("player1Colliders"))
        {
            PlayersCarController player = other.GetComponentInParent<PlayersCarController>();
            if (player != null)
            {
                player.HealingOverTime(false);
                healingTargets.Remove(player);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var player in healingTargets)
        {
            if (player != null)
            {
                player.HealingOverTime(false);
            }
        }
        healingTargets.Clear();
    }
}