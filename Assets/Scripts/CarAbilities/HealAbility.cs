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

        yield return new WaitForSeconds(10); // Wait seconds

        // Turn off the healing field
        Destroy(clone);
    }
}
