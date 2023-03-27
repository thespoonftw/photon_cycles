using System;
using Unity.Netcode;
using UnityEngine;

public class JoinMenuController
{
    private readonly IInputController input;
    private readonly Updater updater;
    private readonly Action joinCallback;

    public JoinMenuController(Resourcer resourcer, IInputController input, NetworkManager network, Updater updater, Action joinCallback)
    {
        this.input = input;
        this.updater = updater;
        this.joinCallback = joinCallback;
        var go = GameObject.Instantiate(resourcer.joinMenuCanvasPrefab);
        network.StartClient();
        updater.OnUpdate += Update;
    }

    private void Update()
    {
        if (input.GetProceedDown())
            Join();
    }

    private void Join()
    {
        updater.OnUpdate -= Update;
        joinCallback.Invoke();
    }
}
