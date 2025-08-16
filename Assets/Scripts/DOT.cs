using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOT : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 5f;
    [SerializeField] private float tickRate = 1f; // Damage every second

    public string audioToPlay;

    private Dictionary<PlayersCarController, Coroutine> activeCoroutines = new();
    private Dictionary<AICarController, Coroutine> activeCoroutinesAI = new();

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController player = other.GetComponentInParent<PlayersCarController>();
        if (player != null && !activeCoroutines.ContainsKey(player))
        {
            Coroutine damageRoutine = StartCoroutine(DealDamageOverTime(player));
            activeCoroutines.Add(player, damageRoutine);
            return;
        }

        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null && !activeCoroutinesAI.ContainsKey(enemy))
        {
            Coroutine AIdamageRoutine = StartCoroutine(DealDamageOverTimeAI(enemy));
            activeCoroutinesAI.Add(enemy, AIdamageRoutine);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController player = other.GetComponentInParent<PlayersCarController>();
        if (player != null && activeCoroutines.ContainsKey(player))
        {
            StopCoroutine(activeCoroutines[player]);
            activeCoroutines.Remove(player);
            return;
        }

        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null && activeCoroutinesAI.ContainsKey(enemy))
        {
            StopCoroutine(activeCoroutinesAI[enemy]);
            activeCoroutinesAI.Remove(enemy);
            return;
        }
    }

    private IEnumerator DealDamageOverTime(PlayersCarController player)
    {
        while (true)
        {
            if (player.health > 0)
            {
                player.health -= damagePerTick;
                GameManager.instance.StartCoroutine(GameManager.instance.playerVFX(player.playerNum, "sparks"));
                player.SendMessage("CheckHealth");
                if (audioToPlay != null)
                {
                    AudioManager.instance.PlaySFX(audioToPlay);
                }
            }
            yield return new WaitForSeconds(tickRate);
        }
    }

    private IEnumerator DealDamageOverTimeAI(AICarController enemy)
    {
        while (true)
        {
            if (enemy.health > 0)
            {
                enemy.health -= damagePerTick;
                GameManager.instance.StartCoroutine(GameManager.instance.playerVFX(enemy.aiNum, "sparks"));
                enemy.SendMessage("CheckHealth");
                if (audioToPlay != null)
                {
                    AudioManager.instance.PlaySFX(audioToPlay);
                }
            }
            yield return new WaitForSeconds(tickRate);
        }
    }
}