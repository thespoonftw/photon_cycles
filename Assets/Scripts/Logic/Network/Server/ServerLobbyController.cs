using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ServerLobbyController
{
    private readonly Game game;
    private readonly Server server;
    private readonly Action startGameCallback;
    private readonly LobbyCanvas lobbyCanvas;
    private readonly ServerBikeManager bikeManager;

    public ServerLobbyController(Game game, Server server, Action startGameCallback)
    {
        this.game = game;
        this.server = server;
        this.startGameCallback = startGameCallback;

        //bikeManager = new ServerBikeManager(game, server);

        game.Mono.OnUpdate += Update;
        server.Clients.OnMessage += Message;
    }

    public void Message(ulong clientId, INetworkSerializable msg)
    {
        if (msg is PlayerJoinedMsg)
        {
            AddPlayer(clientId, (PlayerJoinedMsg)msg);
        }
    }

    public void AddPlayer(ulong clientId, PlayerJoinedMsg msg)
    {
        var controllerType = game.Inputs.GetController(msg.ControllerIndex);
        var player = server.Players.AddPlayer(controllerType, clientId);
        var response = new PlayerJoinedSuccessMsg()
        {
            PlayerId = player.Id,
            ControllerIndex = msg.ControllerIndex,
            Color = player.Color
        };
        server.Clients.ToClient(clientId, response);
        //lobbyCanvas.UpdatePlayerList(); TODO
    }

    private void FinishSetup()
    {
        if (server.Players.GetAllPlayers().Count == 0)
            return;

        GameObject.Destroy(lobbyCanvas.gameObject);
        game.Mono.OnUpdate -= Update;
        server.Clients.OnMessage -= Message;
        startGameCallback.Invoke();
    }

    private void Update()
    {
        if (game.Inputs.LeadInput.GetProceedDown()) 
            FinishSetup();
        
    }
}
