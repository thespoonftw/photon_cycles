using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BikeManager
{
    private Dictionary<Player, BikeController> dict = new();

    private SpawnLocater spawnLocater;
    private Resourcer resourcer;
    private Updater updater;

    public BikeManager(SpawnLocater spawnLocater, Resourcer resourcer, Updater updater)
    {
        this.spawnLocater = spawnLocater;
        this.resourcer = resourcer;
        this.updater = updater;
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
            bike.MoveCamera(CameraValues.GetCameraRect(i, dict.Count));
            i++;
        }
    }

}
