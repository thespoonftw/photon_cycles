using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class ServerController
{
    private readonly Game game;
    private readonly Server server;
    private readonly ServerClient client;

    public ServerController(Game game)
    {
        this.game = game;
        var levels = new LevelManager(game.Network);
        var clients = new ServerClientManager(game.Network);
        server = new Server(new(), levels, clients);
        game.Network.StartHost();
        LoadPracticeLevel();
    }

    private void LoadPracticeLevel()
    {
        server.Levels.LoadLevel(game.Resources.practiceLevel, StartLobby);
    }

    private void StartLobby()
    {
        new ServerLobbyController(game, server, StartLevelSelect);
    }

    private void StartLevelSelect()
    {
        new ServerLevelSelectController(game, server, StartLiveGame);
    }

    private void StartLiveGame()
    {
        new ServerMatchController(game, server, LoadPracticeLevel);
    }

    private void Exit()
    {

    }

    
}
