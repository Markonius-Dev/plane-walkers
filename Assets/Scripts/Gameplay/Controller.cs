using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    // Main script running the game.

    public static Controller instance;
    private void Awake() => instance = this;

    private Data data;

    private void Start()
    {
        data = new Data();

        LoadScene(data.currentScene);    
    }

    private void LoadScene(string scene)
    {
        try
        {
            Debug.Log($"Loading Scene {scene}");
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
        catch
        {
            Debug.LogError($"Couldn't Load Scene {scene}");
        }
    }
}
