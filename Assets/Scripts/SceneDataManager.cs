using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataManager : MonoBehaviour
{
    public static SceneDataManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if(scene.name == "Play")
        {
            GameManager.Instance.Play();
        }
        else
        {

        }
    }

    public void GoToPlayScene(AudioData audioData)
    {
        GameManager.Instance.audioData = audioData;
        SceneManager.LoadScene("Play");
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }


}