using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCar : MonoBehaviour
{
    public int playerNumber;
    public bool isCC = false;

    public Material[] materials;
    public Renderer[] targetRenderers;
    private int currentIndex = 0;

    private void Awake()
    {
        if (playerNumber == 1)
        {
            Button leftButton = GameObject.Find("ConstumeLeft1").GetComponent<Button>();
            Button rightButton = GameObject.Find("ConstumeRight1").GetComponent<Button>();
            Button scanButton = GameObject.Find("Scan1Button").GetComponent<Button>();
            leftButton.onClick.AddListener(PreviousMaterial);
            rightButton.onClick.AddListener(NextMaterial);
            scanButton.onClick.AddListener(RemoveCar);
        }
        if (playerNumber == 2)
        {
            Button leftButton = GameObject.Find("ConstumeLeft2").GetComponent<Button>();
            Button rightButton = GameObject.Find("ConstumeRight2").GetComponent<Button>();
            Button scanButton = GameObject.Find("Scan2Button").GetComponent<Button>();
            leftButton.onClick.AddListener(PreviousMaterial);
            rightButton.onClick.AddListener(NextMaterial);
            scanButton.onClick.AddListener(RemoveCar);
        }
    }

    private void Start()
    {
        if (isCC)
        {
            GameManager.instance.SelectCar(playerNumber, "CrashCourser");
            GameManager.instance.SelectWrap(playerNumber, 1);
        }
    }

    public void NextMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex + 1) % materials.Length;
        GameManager.instance.SelectWrap(playerNumber, currentIndex+1); // Save selection to GameManager
        ApplyMaterial();
    }

    public void PreviousMaterial()
    {
        if (materials.Length == 0) return;

        currentIndex = (currentIndex - 1 + materials.Length) % materials.Length;
        GameManager.instance.SelectWrap(playerNumber, currentIndex+1); // Save selection to GameManager
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
}
