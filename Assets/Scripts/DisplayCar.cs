using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCar : MonoBehaviour
{
    public int playerNumber;

    public Material[] materials;
    public Renderer[] targetRenderers;
    private int currentIndex = 0;

    public void RemoveCar()
    {
        if (playerNumber == 1)
        {
            GameManager.instance.p1Car = null;
        }
        if (playerNumber == 2)
        {
            GameManager.instance.p2Car = null;
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        if (playerNumber == 1)
        {
            Button leftButton = GameObject.Find("ConstumeLeft1").GetComponent<Button>();
            Button rightButton = GameObject.Find("ConstumeRight1").GetComponent<Button>();
            leftButton.onClick.AddListener(PreviousMaterial);
            rightButton.onClick.AddListener(NextMaterial);
        }
        if (playerNumber == 2)
        {
            Button leftButton = GameObject.Find("ConstumeLeft2").GetComponent<Button>();
            Button rightButton = GameObject.Find("ConstumeRight2").GetComponent<Button>();
            leftButton.onClick.AddListener(PreviousMaterial);
            rightButton.onClick.AddListener(NextMaterial);
        }
    }

    public void NextMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex + 1) % materials.Length;
        ApplyMaterial();
    }

    public void PreviousMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex - 1 + materials.Length) % materials.Length;
        ApplyMaterial();
    }

    private void ApplyMaterial()
    {
        Material selectedMaterial = materials[currentIndex];

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
