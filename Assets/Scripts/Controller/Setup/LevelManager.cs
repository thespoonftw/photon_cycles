using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    private readonly NetworkManager network;
    private Action postLoadAction;

    public LevelManager(NetworkManager network, Resourcer resourcer)
    {
        this.network = network;
    }

    public void LoadLevel(string levelName, Action postLoadAction)
    {
        this.postLoadAction = postLoadAction;
        network.SceneManager.OnLoadEventCompleted += FinishLoading;
        network.SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    private void FinishLoading(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        network.SceneManager.OnLoadEventCompleted -= FinishLoading;
        postLoadAction.Invoke();
    }
}
