using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController
{
    private readonly Action levelSelectCallback;
    private readonly LevelManager levelManager;
    private readonly LevelSelectCanvas canvas;
    private readonly Updater updater;
    private readonly IInputController leadInput;
    private readonly List<string> levelNames;

    private int currentSelectionIndex = 0;

    public LevelSelectController(Updater updater, IInputController leadInput, Resourcer resourcer, Action levelSelectCallback, LevelManager levelManager)
    {
        this.levelSelectCallback = levelSelectCallback;
        this.updater = updater;
        this.leadInput = leadInput;
        this.levelManager = levelManager;

        var canvasGo = GameObject.Instantiate(resourcer.levelSelectCanvasPrefab);
        canvas = canvasGo.GetComponent<LevelSelectCanvas>();

        canvas.gameObject.SetActive(true);
        updater.OnUpdate += Update;
        canvas.ChangeSelection(0);
        levelNames = resourcer.levels;
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
        levelManager.LoadLevel(levelName, levelSelectCallback);
    }
}
