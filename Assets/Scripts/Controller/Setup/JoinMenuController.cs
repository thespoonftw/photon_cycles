using System;
using Unity.Netcode;
using UnityEngine;

public class JoinMenuController
{
    private readonly IInputController input;
    private readonly Updater updater;

    public JoinMenuController(Resourcer resourcer, IInputController input, NetworkManager network, Updater updater)
    {
        this.input = input;
        this.updater = updater;
        var go = GameObject.Instantiate(resourcer.joinMenuCanvasPrefab);
        network.StartClient();
    }

    private void Update()
    {
        /*
        if (input.GetStartDown())
            Complete();

        var vert = input.GetVerticalDown();
        if (vert != 0)
            SwitchSelection(-vert);
        */
    }
}
