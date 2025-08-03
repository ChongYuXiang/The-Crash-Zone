using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CementAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject cement;
    [SerializeField]
    private float duration;

    public void ActivateAbility()
    {
        StartCoroutine(AbilityTimer()); // Start timer for ability
    }

    IEnumerator AbilityTimer()
    {
        GameObject clone = (GameObject)Instantiate(cement, gameObject.transform.position, gameObject.transform.rotation);
        AudioManager.instance.PlaySFX("DeployCement");

        yield return new WaitForSeconds(duration); // Wait seconds

        // Remove cement
        Destroy(clone);
    }
}
