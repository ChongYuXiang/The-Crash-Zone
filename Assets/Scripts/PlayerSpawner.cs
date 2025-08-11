using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint1;
    [SerializeField] 
    private Transform spawnPoint2;

    [SerializeField] 
    private GameObject CrashCourser1;
    [SerializeField]
    private GameObject CrashCourser2;
    [SerializeField]
    private GameObject WinchWrangler1;
    [SerializeField]
    private GameObject WinchWrangler2;
    [SerializeField]
    private GameObject SirenSaviour1;
    [SerializeField]
    private GameObject SirenSaviour2;
    [SerializeField]
    private GameObject RoaringRacer1;
    [SerializeField]
    private GameObject RoaringRacer2;
    [SerializeField]
    private GameObject MasterMixer1;
    [SerializeField]
    private GameObject MasterMixer2;

    public bool forVS = true;

    private void Start()
    {
        if (forVS && GameManager.instance.playerCount == "VS")
        {
            // Spawn player 1
            if (GameManager.instance.p1Car == "CrashCourser")
            {
                GameObject clone = (GameObject)Instantiate(CrashCourser1, spawnPoint1.position, spawnPoint1.rotation);
            }
            if (GameManager.instance.p1Car == "WinchWrangler")
            {
                GameObject clone = (GameObject)Instantiate(WinchWrangler1, spawnPoint1.position, spawnPoint1.rotation);
            }
            if (GameManager.instance.p1Car == "SirenSaviour")
            {
                GameObject clone = (GameObject)Instantiate(SirenSaviour1, spawnPoint1.position, spawnPoint1.rotation);
            }
            if (GameManager.instance.p1Car == "RoaringRacer")
            {
                GameObject clone = (GameObject)Instantiate(RoaringRacer1, spawnPoint1.position, spawnPoint1.rotation);
            }
            if (GameManager.instance.p1Car == "MasterMixer")
            {
                GameObject clone = (GameObject)Instantiate(MasterMixer1, spawnPoint1.position, spawnPoint1.rotation);
            }

            // Spawn player 2
            if (GameManager.instance.p2Car == "CrashCourser")
            {
                GameObject clone = (GameObject)Instantiate(CrashCourser2, spawnPoint2.position, spawnPoint2.rotation);
            }
            if (GameManager.instance.p2Car == "WinchWrangler")
            {
                GameObject clone = (GameObject)Instantiate(WinchWrangler2, spawnPoint2.position, spawnPoint2.rotation);
            }
            if (GameManager.instance.p2Car == "SirenSaviour")
            {
                GameObject clone = (GameObject)Instantiate(SirenSaviour2, spawnPoint2.position, spawnPoint2.rotation);
            }
            if (GameManager.instance.p2Car == "RoaringRacer")
            {
                GameObject clone = (GameObject)Instantiate(RoaringRacer2, spawnPoint2.position, spawnPoint2.rotation);
            }
            if (GameManager.instance.p2Car == "MasterMixer")
            {
                GameObject clone = (GameObject)Instantiate(MasterMixer2, spawnPoint2.position, spawnPoint2.rotation);
            }
        }
        if (!forVS && GameManager.instance.playerCount == "Solo")
        {
            // Spawn player 1
            if (GameManager.instance.p1Car == "CrashCourser")
            {
                GameObject clone = (GameObject)Instantiate(CrashCourser1, spawnPoint1.position, spawnPoint1.rotation);
            }
            if (GameManager.instance.p1Car == "WinchWrangler")
            {
                GameObject clone = (GameObject)Instantiate(WinchWrangler1, spawnPoint1.position, spawnPoint1.rotation);
            }
            if (GameManager.instance.p1Car == "SirenSaviour")
            {
                GameObject clone = (GameObject)Instantiate(SirenSaviour1, spawnPoint1.position, spawnPoint1.rotation);
            }
            if (GameManager.instance.p1Car == "RoaringRacer")
            {
                GameObject clone = (GameObject)Instantiate(RoaringRacer1, spawnPoint1.position, spawnPoint1.rotation);
            }
            if (GameManager.instance.p1Car == "MasterMixer")
            {
                GameObject clone = (GameObject)Instantiate(MasterMixer1, spawnPoint1.position, spawnPoint1.rotation);
            }
        }
    }
}
