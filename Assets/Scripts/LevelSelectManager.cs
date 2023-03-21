using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectManager
{
    private readonly Action<int> levelSelectCallback;
    private readonly LevelSelectCanvas canvas;
    private readonly Updater updater;
    private readonly Player leadPlayer;

    private bool disableSelection = false;
    private int currentSelectionIndex = 0;

    private List<int> levelIndexes = new List<int>() { 1, 2, 3 };


    public LevelSelectManager(Updater updater, Player leadPlayer, LevelSelectCanvas canvas, Action<int> levelSelectCallback)
    {
        this.levelSelectCallback = levelSelectCallback;
        this.canvas = canvas;
        this.updater = updater;
        this.leadPlayer = leadPlayer;

        canvas.gameObject.SetActive(true);
        updater.OnUpdate += Update;
        canvas.ChangeSelection(0);
    }

    private void Update()
    {
        var input = leadPlayer.Input.GetXAxis();

        if (input != 0)
            SwitchSelection(input);
        else
            disableSelection = false;

        if (Input.GetKeyDown(KeyCode.Return))
            SelectLevel(levelIndexes[currentSelectionIndex]);        
    }

    private void SwitchSelection(int delta)
    {
        if (disableSelection)
            return;

        disableSelection = true;
        currentSelectionIndex += delta;

        if (currentSelectionIndex >= levelIndexes.Count)
            currentSelectionIndex = 0;

        if (currentSelectionIndex < 0)
            currentSelectionIndex = levelIndexes.Count - 1;

        canvas.ChangeSelection(currentSelectionIndex);
    }

    private void SelectLevel(int levelSelectIndex)
    {
        updater.OnUpdate -= Update;
        canvas.gameObject.SetActive(false);
        levelSelectCallback.Invoke(levelSelectIndex);
    }
}
