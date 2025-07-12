using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostAbility : MonoBehaviour
{
    [SerializeField] private PlayersCarController playerCar;
    [SerializeField] private ParticleSystem flame;
    [SerializeField] private float speed = 50f;
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        flame.Stop();
    }

    public void ActivateAbility()
    {
        AudioManager.instance.PlaySFX("RRBoost");
        playerCar.ApplyInstantSpeedBoost(speed,duration);
        StartCoroutine(FlameEffect());
    }

    IEnumerator FlameEffect()
    {
        flame.Play();
        yield return new WaitForSeconds(duration); // Wait seconds
        flame.Stop();
    }
}
