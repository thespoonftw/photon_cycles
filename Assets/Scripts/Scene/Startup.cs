using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    [SerializeField] MonoManager mono;
    [SerializeField] ResourceManager resourcer;
    [SerializeField] NetworkManager network;

    private void Start()
    {
        var game = new Game(mono, network, resourcer);
        new GameController(game);
    }
}
