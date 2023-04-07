using System;
using UnityEngine;

public enum MainMenuSelection
{
    Play,
    Join,
}

public class MainMenuController
{
    private readonly Game game;
    private readonly Action<MainMenuSelection> callback;
    private readonly MainMenuCanvas canvas;
    private readonly int numberOfOptions = 2;

    private int currentSelectionIndex = 0;

    public MainMenuController(Game game, Action<MainMenuSelection> callback)
    {
        this.game = game;
        this.callback = callback;
        var go = GameObject.Instantiate(game.Resources.mainMenuCanvasPrefab);
        canvas = go.GetComponent<MainMenuCanvas>();
        game.Mono.OnUpdate += Update;
        canvas.ChangeSelection(0);
    }

    private void Update()
    {
        if (game.Inputs.LeadInput.GetProceedDown())
            Complete();

        var vert = game.Inputs.LeadInput.GetVerticalDown();
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
        game.Mono.OnUpdate -= Update;
        GameObject.Destroy(canvas.gameObject);
        callback.Invoke((MainMenuSelection)currentSelectionIndex);
    }

}
