using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    private static void OnStartup()
    {
        if (!SceneManager.GetSceneByBuildIndex(0).isLoaded)
        {
            SceneManager.LoadScene(0);
        }
    }

    [SerializeField] Updater updater;
    [SerializeField] Resourcer resourcer;

    private void Start()
    {
        var gameManager = new GameManager(resourcer, updater);
    }
}
