using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Freeze : MonoBehaviour
{
    [Header("Freeze Settings")]
    public float stayDurationBeforeFreeze = 3f;
    public float freezeDuration = 3f;
    public float slowDownDuration = 2f;
    public float restoreDuration = 2f;

    private Dictionary<PlayersCarController, Coroutine> waitCoroutines;
    private Dictionary<AICarController, Coroutine> AIwaitCoroutines;
    private HashSet<PlayersCarController> freezingNow;
    private HashSet<AICarController> AIfreezingNow;

    private void Awake()
    {
        waitCoroutines = new Dictionary<PlayersCarController, Coroutine>();
        AIwaitCoroutines = new Dictionary<AICarController, Coroutine>();

        freezingNow = new HashSet<PlayersCarController>();
        AIfreezingNow = new HashSet<AICarController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null && !freezingNow.Contains(car) && !waitCoroutines.ContainsKey(car))
        {
            Coroutine waitCoroutine = StartCoroutine(WaitThenFreeze(car));
            waitCoroutines[car] = waitCoroutine;
            return;
        }
        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null && !AIfreezingNow.Contains(enemy) && !AIwaitCoroutines.ContainsKey(enemy))
        {
            Coroutine waitCoroutine = StartCoroutine(WaitThenFreezeAI(enemy));
            AIwaitCoroutines[enemy] = waitCoroutine;
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayersCarController car = other.GetComponentInParent<PlayersCarController>();
        if (car != null)
        {
            if (waitCoroutines.TryGetValue(car, out Coroutine routine))
            {
                if (routine != null) StopCoroutine(routine);
                waitCoroutines.Remove(car);
            }

            if (freezingNow.Contains(car))
            {
                Debug.Log($"[FreezeZone] {car.name} exited zone WHILE freezing.");
            }
            return;
        }

        AICarController enemy = other.GetComponentInParent<AICarController>();
        if (enemy != null)
        {
            if (AIwaitCoroutines.TryGetValue(enemy, out Coroutine routine))
            {
                if (routine != null) StopCoroutine(routine);
                AIwaitCoroutines.Remove(enemy);
            }

            if (AIfreezingNow.Contains(enemy))
            {
                Debug.Log($"[FreezeZone] {enemy.name} exited zone WHILE freezing.");
            }
            return;
        }
    }

    private IEnumerator WaitThenFreeze(PlayersCarController car)
    {
        float timer = 0f;

        while (timer < stayDurationBeforeFreeze)
        {
            if (!IsCarStillInZone(car)) yield break;
            timer += Time.deltaTime;
            yield return null;
        }

        waitCoroutines.Remove(car);
        freezingNow.Add(car);

        yield return StartCoroutine(FreezeSequence(car));

        freezingNow.Remove(car);
    }

    private IEnumerator WaitThenFreezeAI(AICarController enemy)
    {
        float timer = 0f;

        while (timer < stayDurationBeforeFreeze)
        {
            if (!IsAIStillInZone(enemy)) yield break;
            timer += Time.deltaTime;
            yield return null;
        }

        AIwaitCoroutines.Remove(enemy);
        AIfreezingNow.Add(enemy);

        yield return StartCoroutine(AIFreezeSequence(enemy));

        AIfreezingNow.Remove(enemy);
    }

    private IEnumerator FreezeSequence(PlayersCarController car)
    {
        float elapsed = 0f;
        float startSpeed = car.maxSpeed;

        Renderer iceBlock = car.iceCubeRenderer;
        CanvasGroup playerCanvas = car.freezeCanvasGroup;

        // Show ice cube
        if (iceBlock != null)
        {
            iceBlock.gameObject.SetActive(true);
            iceBlock.transform.SetParent(car.transform);
            iceBlock.transform.localPosition = new Vector3(0f, 1.3f, 0f); // Adjust offset if needed
            iceBlock.transform.localRotation = Quaternion.identity;

            StartCoroutine(FadeMesh(iceBlock, 0f, 0.7f, slowDownDuration));
        }

        // Show player's freeze UI
        if (playerCanvas != null)
            StartCoroutine(FadeCanvas(playerCanvas, 0f, 1f, slowDownDuration));

        // Slow down the car gradually
        while (elapsed < slowDownDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slowDownDuration);
            car.maxSpeed = Mathf.Lerp(startSpeed, 0f, t);
            yield return null;
        }

        car.maxSpeed = 0f;
        car.isFrozen = true;

        yield return new WaitForSeconds(freezeDuration);

        // Fade out UI
        if (playerCanvas != null)
            yield return StartCoroutine(FadeCanvas(playerCanvas, 1f, 0f, 1f));

        // Fade out ice cube
        if (iceBlock != null)
            yield return StartCoroutine(FadeMesh(iceBlock, 0.7f, 0f, 1f));

        // Detach and deactivate ice cube
        if (iceBlock != null)
        {
            iceBlock.transform.SetParent(null);
            iceBlock.gameObject.SetActive(false);
        }

        car.isFrozen = false;

        // Restore car speed gradually
        elapsed = 0f;
        while (elapsed < restoreDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / restoreDuration);
            car.maxSpeed = Mathf.Lerp(0f, car.defaultMaxSpeed, t);
            yield return null;
        }

        car.maxSpeed = car.defaultMaxSpeed;
    }

    private IEnumerator AIFreezeSequence(AICarController enemy)
    {
        float elapsed = 0f;
        float startSpeed = enemy.maxSpeed;

        Renderer iceBlock = enemy.iceCubeRenderer;
        CanvasGroup aiCanvas = enemy.freezeCanvasGroup;

        // Show ice cube
        if (iceBlock != null)
        {
            iceBlock.gameObject.SetActive(true);
            iceBlock.transform.SetParent(enemy.transform);
            iceBlock.transform.localPosition = new Vector3(0f, 1.3f, 0f);
            iceBlock.transform.localRotation = Quaternion.identity;

            StartCoroutine(FadeMesh(iceBlock, 0f, 0.7f, slowDownDuration));
        }

        // Show AI freeze UI (if you have one)
        if (aiCanvas != null)
            StartCoroutine(FadeCanvas(aiCanvas, 0f, 1f, slowDownDuration));

        // Slow down gradually
        while (elapsed < slowDownDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slowDownDuration);
            enemy.maxSpeed = Mathf.Lerp(startSpeed, 0f, t);
            yield return null;
        }

        enemy.maxSpeed = 0f;
        enemy.isFrozen = true;

        yield return new WaitForSeconds(freezeDuration);

        // Fade out UI
        if (aiCanvas != null)
            yield return StartCoroutine(FadeCanvas(aiCanvas, 1f, 0f, 1f));

        // Fade out ice cube
        if (iceBlock != null)
            yield return StartCoroutine(FadeMesh(iceBlock, 0.7f, 0f, 1f));

        // Detach and deactivate ice cube
        if (iceBlock != null)
        {
            iceBlock.transform.SetParent(null);
            iceBlock.gameObject.SetActive(false);
        }

        enemy.isFrozen = false;

        // Restore speed gradually
        elapsed = 0f;
        while (elapsed < restoreDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / restoreDuration);
            enemy.maxSpeed = Mathf.Lerp(0f, enemy.defaultMaxSpeed, t);
            yield return null;
        }

        enemy.maxSpeed = enemy.defaultMaxSpeed;
    }


    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float from, float to, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;
        if (!canvasGroup.gameObject.activeSelf)
            canvasGroup.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;

        if (to == 0f)
            canvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator FadeMesh(Renderer rend, float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        Material mat = rend.material;
        Color baseColor = mat.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
            mat.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            yield return null;
        }

        mat.color = new Color(baseColor.r, baseColor.g, baseColor.b, toAlpha);
    }

    private bool IsCarStillInZone(PlayersCarController car)
    {
        Collider zoneCollider = GetComponent<Collider>();
        if (!zoneCollider || !zoneCollider.isTrigger) return false;

        Collider carCollider = car.GetComponentInChildren<Collider>();
        if (!carCollider) return false;

        return zoneCollider.bounds.Intersects(carCollider.bounds);
    }

    private bool IsAIStillInZone(AICarController enemy)
    {
        Collider zoneCollider = GetComponent<Collider>();
        if (!zoneCollider || !zoneCollider.isTrigger) return false;

        Collider enemyCollider = enemy.GetComponentInChildren<Collider>();
        if (!enemyCollider) return false;

        return zoneCollider.bounds.Intersects(enemyCollider.bounds);
    }
}