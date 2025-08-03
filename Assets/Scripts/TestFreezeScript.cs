using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestFreezeScript : MonoBehaviour
{
    [Header("Freeze Settings")]
    public float stayDurationBeforeFreeze = 3f;  // Time player must stay before freezing starts
    public float freezeDuration = 3f;            // Time player remains fully frozen
    public float slowDownDuration = 2f;          // Time to reduce speed to 0
    public float restoreDuration = 2f;           // Time to return to full speed

    private Dictionary<PlayersCarController, Coroutine> waitCoroutines = new Dictionary<PlayersCarController, Coroutine>();
    private HashSet<PlayersCarController> freezingNow = new HashSet<PlayersCarController>();

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null && !freezingNow.Contains(car))
        {
            Debug.Log($"[FreezeZone] {car.name} entered zone. Starting wait timer...");
            Coroutine waitCoroutine = StartCoroutine(WaitThenFreeze(car));
            waitCoroutines[car] = waitCoroutine;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            // If player leaves before freeze activates
            if (waitCoroutines.ContainsKey(car))
            {
                StopCoroutine(waitCoroutines[car]);
                waitCoroutines.Remove(car);
                Debug.Log($"[FreezeZone] {car.name} exited zone BEFORE freeze started.");
            }

            if (freezingNow.Contains(car))
            {
                Debug.Log($"[FreezeZone] {car.name} exited zone WHILE freezing.");
                // We don’t cancel freezing if already started, but you can change that here.
            }
        }
    }

    private IEnumerator WaitThenFreeze(PlayersCarController car)
    {
        float timer = 0f;

        while (timer < stayDurationBeforeFreeze)
        {
            if (!IsCarStillInZone(car)) yield break; // safety check
            timer += Time.deltaTime;
            yield return null;
        }

        // Remove from wait list, move to freezing set
        waitCoroutines.Remove(car);
        freezingNow.Add(car);

        yield return StartCoroutine(FreezeSequence(car));

        // Done freezing
        freezingNow.Remove(car);
    }

    private IEnumerator FreezeSequence(PlayersCarController car)
    {
        float elapsed = 0f;
        float startSpeed = car.maxSpeed;

        // Slow down
        while (elapsed < slowDownDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slowDownDuration);
            car.maxSpeed = Mathf.Lerp(startSpeed, 0f, t);
            Debug.Log($"[FreezeZone] Slowing {car.name}: {car.maxSpeed:F2}");
            yield return null;
        }

        car.maxSpeed = 0f;
        car.isFrozen = true;
        Debug.Log($"[FreezeZone] {car.name} is now FROZEN for {freezeDuration} seconds.");

        yield return new WaitForSeconds(freezeDuration);

        car.isFrozen = false;
        Debug.Log($"[FreezeZone] {car.name} UNFROZEN. Restoring speed...");

        elapsed = 0f;
        while (elapsed < restoreDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / restoreDuration);
            car.maxSpeed = Mathf.Lerp(0f, car.defaultMaxSpeed, t);
            Debug.Log($"[FreezeZone] Restoring {car.name}: {car.maxSpeed:F2}");
            yield return null;
        }

        car.maxSpeed = car.defaultMaxSpeed;
        Debug.Log($"[FreezeZone] {car.name} speed FULLY restored.");
    }

    private bool IsCarStillInZone(PlayersCarController car)
    {
        // This checks if the car's collider is still overlapping the trigger
        Collider zoneCollider = GetComponent<Collider>();
        if (!zoneCollider || !zoneCollider.isTrigger) return false;

        Collider carCollider = car.GetComponentInChildren<Collider>();
        if (!carCollider) return false;

        return zoneCollider.bounds.Intersects(carCollider.bounds);
    }
}

