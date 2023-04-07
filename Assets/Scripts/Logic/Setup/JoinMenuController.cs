using System;
using Unity.Netcode;
using UnityEngine;

public class JoinMenuController
{
    private readonly Game game;
    private readonly Action joinCallback;
    private readonly JoinMenuCanvas canvas;

    private bool startedClient = false;

    public JoinMenuController(Game game, Action joinCallback)
    {
        this.game = game;
        this.joinCallback = joinCallback;
        var go = GameObject.Instantiate(game.Resources.joinMenuCanvasPrefab);
        canvas = go.GetComponent<JoinMenuCanvas>();
        game.Mono.OnUpdate += Update;
    }

    private void Update()
    {
        if (!startedClient && game.Inputs.LeadInput.GetProceedDown())
            Join();

        if (game.Network.LocalClient != null && game.Network.LocalClient.PlayerObject != null)
            Joined();
    }

    private void Join()
    {
        game.Account.Name = canvas.GetUsername();
        game.Network.StartClient();
        startedClient = true;
    }

    private void Joined()
    {
        game.Mono.OnUpdate -= Update;
        joinCallback.Invoke();
    }
}
