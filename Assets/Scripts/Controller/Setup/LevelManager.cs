using System;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelManager
{
    public List<string> LevelNames { get; private set; }

    private const int PRACTICE_SCENE_INDEX = 1;

    private readonly NetworkManager network;
    private readonly string practiceScene; 

    private Action postLoadAction;

    public LevelManager(NetworkManager network)
    {
        this.network = network;

        var scenes = EditorBuildSettings.scenes;
        LevelNames = new List<string>();

        for (int i=1; i<scenes.Length; i++)
        {
            var path = scenes[i].path;
            var name = Path.GetFileNameWithoutExtension(path);

            LevelNames.Add(name);
            if (i == PRACTICE_SCENE_INDEX)
                practiceScene = name;
        }
    }

    public void LoadLevel(string levelName, Action postLoadAction)
    {
        this.postLoadAction = postLoadAction;
        network.SceneManager.OnLoadEventCompleted += FinishLoading;
        network.SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void LoadPracticeLevel(Action postLoadAction)
    {
        LoadLevel(practiceScene, postLoadAction);
    }

    private void FinishLoading(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        network.SceneManager.OnLoadEventCompleted -= FinishLoading;
        postLoadAction.Invoke();
    }
}
