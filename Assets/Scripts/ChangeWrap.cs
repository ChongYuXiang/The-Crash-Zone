using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWrap : MonoBehaviour
{
    public int playerNumber;

    public Material[] materials;
    public Renderer[] targetRenderers;

    private void Start()
    {
        Material selectedMaterial = materials[0];
        if (playerNumber == 1)
        {
            selectedMaterial = materials[GameManager.instance.p1Wrap - 1];
        }
        if (playerNumber == 2)
        {
            selectedMaterial = materials[GameManager.instance.p2Wrap - 1];
        }
        if (targetRenderers != null && targetRenderers.Length > 0)
        {
            foreach (Renderer rend in targetRenderers)
            {
                if (rend != null)
                    rend.material = selectedMaterial;
            }
        }
    }
}
