using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStartLight : MonoBehaviour
{
    public Material RedLight;
    public Material YellowLight;
    public Material GreenLight;
    //public Material NoLight;

    public float switchDuration = 1f;

    private Renderer StartLightRenderer;

    // Start is called before the first frame update
    void Start()
    {
        StartLightRenderer = GetComponent<Renderer>();
        StartCoroutine(ChangeLightTexture());
    }

    private IEnumerator ChangeLightTexture()
    {
        while (true)
        {
            StartLightRenderer.material = RedLight;
            Debug.Log("Red Light");
            yield return new WaitForSeconds(switchDuration);

            StartLightRenderer.material = YellowLight;
            Debug.Log("yellow Light");
            yield return new WaitForSeconds(switchDuration);

            StartLightRenderer.material = GreenLight;
            Debug.Log("Green Light");
            yield return new WaitForSeconds(switchDuration);

            /*StartLightRenderer.material = NoLight;
            Debug.Log("Red Light");
            yield return new WaitForSeconds(switchDuration);*/
        }
    }
}
