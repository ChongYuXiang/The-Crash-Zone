using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceStartLight : MonoBehaviour
{
    public Material RedLight;
    public Material YellowLight;
    public Material GreenLight;

    public TextMeshProUGUI countdown;

    public Renderer[] StartLightRenderers;

    private PlayersCarController[] allCars;

    void Start()
    {
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

        yield return new WaitForSeconds(1);
        //countdown.text = "3";
        yield return new WaitForSeconds(1);
        if (StartLightRenderers != null && StartLightRenderers.Length > 0)
        {
            foreach (Renderer rend in StartLightRenderers)
            {
                if (rend != null)
                    rend.material = RedLight;
            }
        }
        Debug.Log("Red Light");
        //countdown.text = "2";
        yield return new WaitForSeconds(1);

        if (StartLightRenderers != null && StartLightRenderers.Length > 0)
        {
            foreach (Renderer rend in StartLightRenderers)
            {
                if (rend != null)
                    rend.material = YellowLight;
            }
        }
        Debug.Log("Yellow Light");
        //countdown.text = "1";
        yield return new WaitForSeconds(1);

        if (StartLightRenderers != null && StartLightRenderers.Length > 0)
        {
            foreach (Renderer rend in StartLightRenderers)
            {
                if (rend != null)
                    rend.material = GreenLight;
            }
        }
        Debug.Log("Green Light");
        //countdown.text = "GO";
        yield return new WaitForSeconds(0.2f);

        // Unfreeze all cars
        foreach (var car in allCars)
        {
            car.isFrozen = false;
        }

        // Start the race timer
        if (RaceTimer.instance != null)
        {
            RaceTimer.instance.StartTimer();

        }
    }
}

