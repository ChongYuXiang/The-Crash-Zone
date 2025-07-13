using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStartLight : MonoBehaviour
{
    public Texture RLight;
    public Texture RYLight;
    public Texture YLight;
    public Texture YGLight;
    public Texture GLight;

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
            StartLightRenderer.material.mainTexture = RLight;
            Debug.Log("Red Light");
            yield return new WaitForSeconds(switchDuration);

            StartLightRenderer.material.mainTexture = RYLight;
            Debug.Log("RY Light");
            yield return new WaitForSeconds(switchDuration);

            StartLightRenderer.material.mainTexture = YLight;
            Debug.Log("Yellow Light");
            yield return new WaitForSeconds(switchDuration);

            StartLightRenderer.material.mainTexture = YGLight;
            Debug.Log("YG Light");
            yield return new WaitForSeconds(switchDuration);

            StartLightRenderer.material.mainTexture = GLight;
            Debug.Log("Green Light");
            yield return new WaitForSeconds(switchDuration);
        }
    }
}
