using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOT : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 5f;
    [SerializeField] private float tickRate = 1f; // Damage every second

    private Dictionary<PlayersCarController, Coroutine> activeCoroutines = new();

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController player = other.GetComponentInParent<PlayersCarController>();
        if (player != null && !activeCoroutines.ContainsKey(player))
        {   
            Coroutine damageRoutine = StartCoroutine(DealDamageOverTime(player));
            activeCoroutines.Add(player, damageRoutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController player = other.GetComponentInParent<PlayersCarController>();
        if (player != null && activeCoroutines.ContainsKey(player))
        {
            StopCoroutine(activeCoroutines[player]);
            activeCoroutines.Remove(player);
        }
    }

    private IEnumerator DealDamageOverTime(PlayersCarController player)
    {
        while (true)
        {
            if (player.health > 0)
            {
                player.health -= damagePerTick;
                player.SendMessage("CheckHealth");
                //AudioManager.instance.PlaySFX("-"); if u wanna add sound
            }
            yield return new WaitForSeconds(tickRate);
        }
    }
}