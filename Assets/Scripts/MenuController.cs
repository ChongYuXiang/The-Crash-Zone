using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    public GameObject playerCam;
    public int camMoveSpd;
    public int camRotateSpd;
    public Transform camPos1;
    public Transform camPos2;
    public Transform camPos3;

    private string moveType;
    private Quaternion lookRotation;
    private Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        if (moveType == "MenuToOptions")
        {
            //rotate camera to be the same as the target
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, camPos2.rotation, Time.deltaTime * camRotateSpd);

            //move camera towards target
            playerCam.transform.position = Vector3.MoveTowards(playerCam.transform.position, camPos2.transform.position, camMoveSpd * Time.deltaTime);
        }
        if (moveType == "BackToMenu")
        {
            //rotate camera to be the same as the target
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, camPos1.rotation, Time.deltaTime * camRotateSpd);

            playerCam.transform.position = Vector3.MoveTowards(playerCam.transform.position, camPos1.transform.position, camMoveSpd * Time.deltaTime);
        }
        if (moveType == "MenuToQuit")
        {
            //rotate camera to be the same as the target
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, camPos3.rotation, Time.deltaTime * camRotateSpd);

            playerCam.transform.position = Vector3.MoveTowards(playerCam.transform.position, camPos3.transform.position, camMoveSpd * Time.deltaTime);
        }
    }

    public void MenuToOptions()
    {
        moveType = "MenuToOptions";
    }
    public void BackToMenu()
    {
        moveType = "BackToMenu";
    }
    public void MenuToQuit()
    {
        moveType = "MenuToQuit";
    }

    // Quit Button
    public void QuitGame()
    {
        // Close unity build
        Application.Quit();
    }

    // Tell audio manager to toggle BGM
    public void BGMVolume(float volume)
    {
        AudioManager.instance.BGMVolume(volume);
    }
    // Tell audio manager to toggle SFX
    public void SFXVolume(float volume)
    {
        AudioManager.instance.SFXVolume(volume);
    }
}
