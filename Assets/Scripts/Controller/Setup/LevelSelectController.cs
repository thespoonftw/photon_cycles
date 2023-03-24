using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectController
{
    private readonly Action<int> levelSelectCallback;
    private readonly LevelSelectCanvas canvas;
    private readonly Updater updater;
    private readonly IInputController leadInput;
    private readonly List<int> levelIndexes = new List<int>() { 1, 2, 3 };

    private int currentSelectionIndex = 0;

    public LevelSelectController(Updater updater, IInputController leadInput, Resourcer resourcer, Action<int> levelSelectCallback)
    {
        this.levelSelectCallback = levelSelectCallback;
        this.updater = updater;
        this.leadInput = leadInput;

        var canvasGo = GameObject.Instantiate(resourcer.levelSelectCanvasPrefab);
        canvas = canvasGo.GetComponent<LevelSelectCanvas>();

        canvas.gameObject.SetActive(true);
        updater.OnUpdate += Update;
        canvas.ChangeSelection(0);
    }

    private void Update()
    {
        var hori = leadInput.GetHorizontalDown();
        if (hori != 0)
            SwitchSelection(hori);

        if (leadInput.GetProceedDown())
            SelectLevel(levelIndexes[currentSelectionIndex]);        
    }

    private void SwitchSelection(int delta)
    {
        currentSelectionIndex = (currentSelectionIndex + delta).Mod(levelIndexes.Count);
        canvas.ChangeSelection(currentSelectionIndex);
    }

    private void SelectLevel(int levelSelectIndex)
    {
        updater.OnUpdate -= Update;
        canvas.gameObject.SetActive(false);
        GameObject.Destroy(canvas.gameObject);
        levelSelectCallback.Invoke(levelSelectIndex);
    }
}
