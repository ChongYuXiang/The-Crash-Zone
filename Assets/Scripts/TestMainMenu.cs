using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMainMenu : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private string sceneToLoad = "transition scene 2";

    public void Play()
    {
        sceneController.LoadScene(1);
    }

    
}
