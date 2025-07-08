using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject healingField;

    public void ActivateAbility(int playerIndex)
    {
        StartCoroutine(AbilityTimer()); // Start timer for ability
    }

    IEnumerator AbilityTimer()
    {
        GameObject clone = (GameObject)Instantiate(healingField, gameObject.transform.position, gameObject.transform.rotation);
        AudioManager.instance.PlaySFX("DeployHeal");

        yield return new WaitForSeconds(9.8f); // Wait seconds

        AudioManager.instance.PlaySFX("HealEnd");


        yield return new WaitForSeconds(0.2f); // Wait seconds

        // Turn off the healing field
        Destroy(clone);
    }
}
