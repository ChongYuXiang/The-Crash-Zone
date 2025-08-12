using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;


    [SerializeField]
    private float sceneFadeDuration;

    private SceneFade sceneFade;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        sceneFade = GetComponentInChildren<SceneFade>();
        sceneFade.gameObject.SetActive(false);
    }

    public void LoadScene(int sceneIndex)
    {
        sceneFade.gameObject.SetActive(true);
        StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    private IEnumerator LoadSceneCoroutine(int index)
    {
        yield return sceneFade.FadeOutCoroutine(sceneFadeDuration);
        yield return SceneManager.LoadSceneAsync(index);

        GameManager.instance.gameOver = false;

        yield return sceneFade.FadeInCoroutine(sceneFadeDuration);
        sceneFade.gameObject.SetActive(false);
    }
}
