using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStartLight : MonoBehaviour
{
    public Material RedLight;
    public Material YellowLight;
    public Material GreenLight;

    public float switchDuration = 1f;
    private Renderer StartLightRenderer;

    private PlayersCarController[] allCars;

    void Start()
    {
        StartLightRenderer = GetComponent<Renderer>();

        StartCoroutine(ChangeLightTexture());
    }   

    private IEnumerator ChangeLightTexture()
    {
        yield return new WaitForSeconds(0.05f);
        allCars = FindObjectsOfType<PlayersCarController>(); // Find cars

        // Freeze all cars
        foreach (var car in allCars)
        {
            car.isFrozen = true;
        }

        yield return new WaitForSeconds(2);
        StartLightRenderer.material = RedLight;
        Debug.Log("Red Light");
        yield return new WaitForSeconds(switchDuration);

        StartLightRenderer.material = YellowLight;
        Debug.Log("Yellow Light");
        yield return new WaitForSeconds(switchDuration);

        StartLightRenderer.material = GreenLight;
        Debug.Log("Green Light");
        yield return new WaitForSeconds(switchDuration);

        // Unfreeze all cars
        foreach (var car in allCars)
        {
            car.isFrozen = false;
        }

        // Start the race timer
        if (RaceTimer.instance != null)
            RaceTimer.instance.StartTimer();
    }
}

