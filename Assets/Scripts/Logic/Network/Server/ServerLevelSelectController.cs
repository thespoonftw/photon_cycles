using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerLevelSelectController
{
    private readonly Action levelSelectCallback;
    private readonly Game game;
    private readonly LevelManager levels;
    private readonly LevelSelectCanvas canvas;
    private readonly MonoManager updater;
    private readonly IInputController leadInput;
    private readonly List<string> levelNames;

    private int currentSelectionIndex = 0;

    public ServerLevelSelectController(Game game, Server server, Action levelSelectCallback)
    {
        this.levelSelectCallback = levelSelectCallback;
        this.levels = server.Levels;
        this.game = game;

        var canvasGo = GameObject.Instantiate(game.Resources.levelSelectCanvasPrefab);
        canvas = canvasGo.GetComponent<LevelSelectCanvas>();

        canvas.gameObject.SetActive(true);
        updater.OnUpdate += Update;
        canvas.ChangeSelection(0);
        levelNames = game.Resources.levels;
    }

    private void Update()
    {
        var hori = leadInput.GetHorizontalDown();
        if (hori != 0)
            SwitchSelection(hori);

        if (leadInput.GetProceedDown())
            SelectLevel(levelNames[currentSelectionIndex]);        
    }

    private void SwitchSelection(int delta)
    {
        currentSelectionIndex = (currentSelectionIndex + delta).Mod(levelNames.Count);
        canvas.ChangeSelection(currentSelectionIndex);
    }

    private void SelectLevel(string levelName)
    {
        updater.OnUpdate -= Update;
        canvas.gameObject.SetActive(false);
        GameObject.Destroy(canvas.gameObject);
        levels.LoadLevel(levelName, levelSelectCallback);
    }
}
