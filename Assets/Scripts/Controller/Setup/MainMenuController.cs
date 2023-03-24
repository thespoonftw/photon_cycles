using System;
using UnityEngine;

public enum MainMenuSelection
{
    Play,
    Join,
}

public class MainMenuController
{
    private readonly Updater updater;
    private readonly Action<MainMenuSelection> callback;
    private readonly MainMenuCanvas canvas;
    private readonly IInputController input;
    private readonly int numberOfOptions = 2;

    private int currentSelectionIndex = 0;

    public MainMenuController(Resourcer resourcer, Updater updater, IInputController input, Action<MainMenuSelection> callback)
    {
        this.updater = updater;
        this.callback = callback;
        this.input = input;
        var go = GameObject.Instantiate(resourcer.mainMenuCanvasPrefab);
        canvas = go.GetComponent<MainMenuCanvas>();
        updater.OnUpdate += Update;
        canvas.ChangeSelection(0);
    }

    private void Update()
    {
        if (input.GetProceedDown())
            Complete();

        var vert = input.GetVerticalDown();
        if (vert != 0)
            SwitchSelection(-vert);
    }

    private void SwitchSelection(int delta)
    {
        currentSelectionIndex = (currentSelectionIndex + delta).Mod(numberOfOptions);
        canvas.ChangeSelection(currentSelectionIndex);
    }

    private void Complete()
    {
        updater.OnUpdate -= Update;
        GameObject.Destroy(canvas.gameObject);
        callback.Invoke((MainMenuSelection)currentSelectionIndex);
    }

}
