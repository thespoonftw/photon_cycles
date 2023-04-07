using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class BikeManager
{
    /*
    private const float SMALL_SCREEN_FACTOR = 0.75f;

    private readonly Game game;
    private readonly bool isSmallerScreen;
    private readonly ClientToServer networker;

    private readonly List<BikeController> bikes = new();
    private readonly List<Player> players = new();

    public BikeManager(Game game, bool isSmallerScreen)
    {
        this.game = game;
        this.isSmallerScreen = isSmallerScreen;

        networker = game.Network.LocalClient.PlayerObject.GetComponent<ClientToServer>();
        networker.Init(this);
    }

    public void AddPlayer(Player player)
    {
        var playerIndex = players.Count();
        players.Add(player);
        networker.ToServerRpc(playerIndex);
    }

    public void RemovePlayer(Player player)
    {
        RemoveBike(player);
        RefreshCameras();
    }

    public void BikeSpawned(int playerIndex, GameObject go)
    {
        var player = players[playerIndex];
        var bike = new BikeController(player, resourcer, updater, go);
        bikes.Add(bike);
        RefreshCameras();
        bike.StartMoving();
    }

    private void RemoveBike(Player player)
    {
        var bike = dict[player];
        bike.Remove();
        dict.Remove(player);
    }

    private void RefreshCameras()
    {
        int i = 0;
        foreach (var bike in bikes)
        {
            bike.MoveCamera(GetRect(i));
            i++;
        }
    }

    private Rect GetRect(int screenIndex)
    {
        var r = CameraValues.GetCameraRect(screenIndex, bikes.Count);

        if (!isSmallerScreen) 
            return r;

        var xOffset = (1 - SMALL_SCREEN_FACTOR) + (r.x * SMALL_SCREEN_FACTOR);
        var yOffset = (1 - SMALL_SCREEN_FACTOR) / 2f + (r.y * SMALL_SCREEN_FACTOR);

        return new Rect(xOffset, yOffset, r.width * SMALL_SCREEN_FACTOR, r.height * SMALL_SCREEN_FACTOR);
    }
    */
}
