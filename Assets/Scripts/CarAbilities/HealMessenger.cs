using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;

public class HealMessenger : MonoBehaviour
{
    private HashSet<PlayersCarController> healingTargets = new HashSet<PlayersCarController>();
    private HashSet<AICarController> AIhealingTargets = new HashSet<AICarController>();

    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player2Colliders") || other.CompareTag("player1Colliders"))
        {
            PlayersCarController player = other.GetComponentInParent<PlayersCarController>();
            if (player != null)
            {
                player.HealingOverTime(true);
                healingTargets.Add(player);
                StartCoroutine(FadeAudio(audioSource, 1f, AudioManager.instance.SFXSource.volume / 1));
                return;
            }
        }
        if (other.CompareTag("enemyCollider"))
        {
            AICarController enemy = other.GetComponentInParent<AICarController>();
            if (enemy != null)
            {
                enemy.HealingOverTime(true);
                AIhealingTargets.Add(enemy);
                StartCoroutine(FadeAudio(audioSource, 1f, AudioManager.instance.SFXSource.volume / 1));
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

                if (healingTargets.Count == 0)
                {
                    StartCoroutine(FadeAudio(audioSource, 1f, 0));
                }
            }
        }
        if (other.CompareTag("enemyCollider"))
        {
            AICarController enemy = other.GetComponentInParent<AICarController>();
            if (enemy != null)
            {
                enemy.HealingOverTime(false);
                AIhealingTargets.Remove(enemy);

                if (AIhealingTargets.Count == 0)
                {
                    StartCoroutine(FadeAudio(audioSource, 1f, 0));
                }
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

        foreach (var enemy in AIhealingTargets)
        {
            if (enemy != null)
            {
                enemy.HealingOverTime(false);
            }
        }
        healingTargets.Clear();
        AIhealingTargets.Clear();
    }

    IEnumerator FadeAudio(AudioSource source, float duration, float targetVolume)
    {
        float time = 0f;
        float startingVol = source.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startingVol, targetVolume, time/duration);
            yield return null;
        }

        yield break;
    }
}