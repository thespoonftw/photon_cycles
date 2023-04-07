using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Unity.Netcode;
using UnityEngine;

public class LobbyController
{
    private readonly Game game;
    private readonly Client client;
    private readonly List<IInputController> inactiveControllers;
    private readonly Dictionary<Player, float> holdDownTime = new();
    private readonly BikeManager bikeManager;
    private readonly LobbyCanvas lobbyCanvas;

    private const float HOLD_DOWN_TIME_SEC = 2f;

    public LobbyController(Game game, Client client)
    {
        this.game = game;
        this.client = client;
        inactiveControllers = game.Inputs.GetAllControllers();
        //bikeManager = new BikeManager(game, true);
        game.Mono.OnUpdate += Update;

        var canvasGo = GameObject.Instantiate(game.Resources.lobbyCanvasPrefab);
        lobbyCanvas = canvasGo.GetComponent<LobbyCanvas>();

        client.OnMessage += Message;
    }

    // TODO call this
    public void Dispose()
    {
        client.OnMessage -= Message;
    }

    private void Message(INetworkSerializable msg)
    {
        if (msg is PlayerJoinedSuccessMsg)
        {
            AddPlayerSuccess((PlayerJoinedSuccessMsg)msg);
        }
    }

    private void Update()
    {
        // add controllers that have movement
        foreach (var c in inactiveControllers)
        {
            if (c.GetHorizontal() != 0)
                AddPlayer(c);
        }

        // measure hold down time
        game.Players.GetAllPlayers().ForEach(p => MeasureHoldDownTime(p));

        // remove controllers with sufficient hold down time
        holdDownTime.Where(pair => pair.Value > HOLD_DOWN_TIME_SEC).Select(pair => pair.Key).ToList().ForEach(j => RemovePlayer(j));
    }

    private void AddPlayer(IInputController controller)
    {
        inactiveControllers.Remove(controller);
        var controllerIndex = game.Inputs.GetControllerIndex(controller);

        var msg = new PlayerJoinedMsg()
        {
            ControllerIndex = controllerIndex
        };
        client.ToServer(msg);
    }

    public void AddPlayerSuccess(PlayerJoinedSuccessMsg msg)
    {
        var controller = game.Inputs.GetController(msg.ControllerIndex);
        var player = game.Players.AddPlayer(msg.PlayerId, controller, msg.Color);
        holdDownTime.Add(player, 0);
        //bikeManager.AddPlayer(player);
        //bike.StartMoving();
        //lobbyCanvas.UpdatePlayerList(players);
    }

    private void RemovePlayer(Player player)
    {       
        //availableColors.Add(player.Color);
        //players.Remove(player);
        //allControllers.Add(player.Input);
        //holdDownTime.Remove(player);
        //bikeManager.RemovePlayer(player);
        //lobbyCanvas.UpdatePlayerList(players);
    }

    private void MeasureHoldDownTime(Player player)
    {
        if (player.Input.GetAction())
        {
            holdDownTime[player] += Time.deltaTime;            
        }
        else
        {
            holdDownTime[player] = 0;
        }
    }
}
