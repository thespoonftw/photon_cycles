using System;
using UnityEngine;

public class TitleScreenController
{
    private readonly Game game;
    private readonly Action callback;
    private readonly GameObject canvasGo;

    public TitleScreenController(Game game, Action callback)
    {
        this.game = game;
        this.callback = callback;
        canvasGo = GameObject.Instantiate(game.Resources.titleScreenCanvasPrefab);
        game.Mono.OnUpdate += Update;
    }

    private void Update()
    {
        foreach (var c in game.Inputs.GetAllControllers())
        {
            if (c.GetProceedDown())
            {
                game.Inputs.LeadInput = c;
                Complete(c);
                return;
            }
        }
    }

    private void Complete(IInputController controller)
    {
        game.Mono.OnUpdate -= Update;
        GameObject.Destroy(canvasGo);
        callback.Invoke();
    }
}
