using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BikeManager : MonoBehaviour
{
    [SerializeField] CameraLoader cameraLoader;
    [SerializeField] GameObject bikePrefab;

    private List<Player> players = new();

    private SpawnLocater spawnLocater;

    public void Init(SpawnLocater spawnLocater)
    {
        this.spawnLocater = spawnLocater;
    }

    public void CreateBikeForPlayer(Player player)
    {
        CreateBike(player);
        RemoveCameras();
        players.Add(player);
        CreateCameras();
    }

    public void CreateBikesForPlayers(List<Player> newPlayers)
    {
        newPlayers.ForEach(p => CreateBike(p));
        RemoveCameras();
        players.AddRange(newPlayers);
        CreateCameras();
    }

    public void RemoveBikeForPlayer(Player player)
    {
        RemoveBike(player);
        RemoveCameras();
        players.Remove(player);
        CreateCameras();
    }

    public void RemoveAllPlayers()
    {
        players.ForEach(p => RemoveBike(p));
        RemoveCameras();
        players = new();
    }

    private void CreateBike(Player player)
    {
        var spawn = spawnLocater.GetNextSpawn();
        var go = Instantiate(bikePrefab, spawn.position, spawn.rotation);
        var bikeController = go.GetComponent<BikeController>();
        bikeController.Init(player, spawn);
        player.SetBike(bikeController);
    }

    private void RemoveBike(Player player)
    {
        var bike = player.Bike;
        Destroy(bike.gameObject);
        player.RemoveBike();
    }

    private void RemoveCameras()
    {
        players.Where(p => p.Camera != null).ToList().ForEach(p => Destroy(p.Camera.gameObject));
    }

    private void CreateCameras()
    {
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            var cameraParent = player.Bike.GetCameraTransform();
            var cameraPrefab = cameraLoader.GetCameraPrefab(i, players.Count);
            var cameraGo = Instantiate(cameraPrefab, cameraParent.position, cameraParent.rotation, cameraParent.transform);
            var camera = cameraGo.GetComponent<Camera>();
            camera.fieldOfView = 70;
            player.SetCamera(camera);
        }
    }

}
