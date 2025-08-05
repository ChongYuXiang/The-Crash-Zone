using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject playerCam;
    public int camMoveSpd;
    public int camRotateSpd;
    public Transform camPos1;
    public Transform camPos2;
    public Transform camPos3;

    private string moveType;
    private string gamemode;
    private string playertype;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AudioManager.instance.PlayBGM("MainMenu");
        }
    }

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
        if (moveType == "MenuToPlay")
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
    public void MenuToPlay()
    {
        moveType = "MenuToPlay";
    }

    // Button interaction to save gamemode as "Arena" or "Racing"
    public void GameSelect(string mode)
    {
        gamemode = mode;
    }
    // Button interaction to save player type as "VS" or "Solo"
    public void PlayerSelect(string type)
    {
        playertype = type;
        GameManager.instance.SelectGameMode(gamemode, playertype);
        SceneController.instance.LoadScene(1);
    }

    public void ChangeScenes(int index)
    {
        SceneController.instance.LoadScene(index);
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

    // Tell audio manager to play sound
    public void PlayAudio(string audioName)
    {
        AudioManager.instance.PlaySFX(audioName);
    }
}
