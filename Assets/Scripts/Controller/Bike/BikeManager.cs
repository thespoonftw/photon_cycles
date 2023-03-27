using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BikeManager
{
    private const float SMALL_SCREEN_FACTOR = 0.75f;

    private readonly Dictionary<Player, BikeController> dict = new();
    private readonly SpawnLocater spawnLocater;
    private readonly Resourcer resourcer;
    private readonly Updater updater;
    private readonly bool isSmallerScreen;

    public BikeManager(Resourcer resourcer, Updater updater, bool isSmallerScreen)
    {
        this.resourcer = resourcer;
        this.updater = updater;
        this.isSmallerScreen = isSmallerScreen;

        spawnLocater = updater.FindSpawnLocater();
    }

    public List<BikeController> GetBikes()
    {
        return dict.Values.ToList();
    }

    public BikeController CreateBikeForPlayer(Player player)
    {
        var bike = CreateBike(player);
        RefreshCameras();
        return bike;
    }

    public List<BikeController> CreateBikesForPlayers(List<Player> newPlayers)
    {
        newPlayers.ForEach(p => CreateBike(p));
        RefreshCameras();
        return GetBikes();
    }

    public void RemoveBikeForPlayer(Player player)
    {
        RemoveBike(player);
        RefreshCameras();
    }

    public void RemoveAllBikes()
    {
        dict.Keys.ToList().ForEach(p => RemoveBike(p));
    }

    private BikeController CreateBike(Player player)
    {
        var spawn = spawnLocater.GetNextSpawn();
        var bike = new BikeController(player, spawn, resourcer, updater);
        dict.Add(player, bike);
        return bike;
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
        foreach (var bike in dict.Values)
        {
            bike.MoveCamera(GetRect(i));
            i++;
        }
    }

    private Rect GetRect(int screenIndex)
    {
        var r = CameraValues.GetCameraRect(screenIndex, dict.Count);

        if (!isSmallerScreen) 
            return r;

        var xOffset = (1 - SMALL_SCREEN_FACTOR) + (r.x * SMALL_SCREEN_FACTOR);
        var yOffset = (1 - SMALL_SCREEN_FACTOR) / 2f + (r.y * SMALL_SCREEN_FACTOR);

        return new Rect(xOffset, yOffset, r.width * SMALL_SCREEN_FACTOR, r.height * SMALL_SCREEN_FACTOR);
    }
}
