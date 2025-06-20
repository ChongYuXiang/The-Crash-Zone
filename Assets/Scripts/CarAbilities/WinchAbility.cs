using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WinchAbility : MonoBehaviour
{
    [SerializeField]
    private LineRenderer ropeLine; // Rope visual

    public GameObject winchMesh; // Winch model on the car
    public GameObject selfPlayer; // For player's rigidbody

    private GameObject ropeTarget; // Pivot for rope to attach to
    private GameObject targetPlayer; // For enemy player's rigidbody

    public int pullStrength = 0; // Physics pull
    public int moveStrength = 0; // Manul movement pull

    void LateUpdate() // At the end of each frame
    {
        if (ropeTarget != null) // If there is a current target selected,
        {
            // Update rope visual
            ropeLine.positionCount = 2; // 2 points, one for start and one for end position
            ropeLine.SetPosition(0, gameObject.transform.position); // Set start position to be self
            ropeLine.SetPosition(1, ropeTarget.transform.position); // Set end position to be at the target pivot


            Rigidbody targetRb = targetPlayer.GetComponent<Rigidbody>();
            Vector3 pullDirection = (ropeLine.transform.position - ropeTarget.transform.position).normalized;

            targetRb.AddForce(pullDirection * pullStrength * Time.deltaTime, ForceMode.Acceleration);


            targetPlayer.transform.position = Vector3.MoveTowards(targetPlayer.transform.position, selfPlayer.transform.position, moveStrength * Time.deltaTime);
            selfPlayer.transform.position = Vector3.MoveTowards(selfPlayer.transform.position, targetPlayer.transform.position, moveStrength * Time.deltaTime);

        }
        else // No current target,
        {
            // Remove rope visual
            ropeLine.positionCount = 0;
        }
    }

    public void ActivateAbility(int playerIndex) // Call from player controller to activate the ability
    {
        ropeTarget = GameObject.Find("player" + playerIndex + "Target"); // Find the player target
        targetPlayer = ropeTarget.transform.parent.gameObject; // Get the target's rigidbody from it's parent
        winchMesh.SetActive(false); // Hide the winch model
        StartCoroutine(AbilityTimer()); // Start timer for ability
    }

    IEnumerator AbilityTimer()
    {
        yield return new WaitForSeconds(4); // Wait 4 seconds

        // Reset targets to null to stop the Update()
        ropeTarget = null;
        targetPlayer = null;

        winchMesh.SetActive(true); // Unhide the winch model
    }
}
