using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ServerBikeManager
{
    /*
    private readonly SpawnLocater spawnLocater;
    private readonly GameObject bikePrefab;

    public ServerBikeManager(Game game, Server host)
    {
        spawnLocater = game.Mono.FindSpawnLocater();
        bikePrefab = game.Resources.bikePrefab;

        game.Network.ConnectedClientsList.Select(c => c.PlayerObject.GetComponent<ClientToServer>()).ToList().ForEach(
            n => n.Init(this)
        );
    }

    public void SpawnBike(int playerIndex, ClientToServer caller)
    {
        var spawn = spawnLocater.GetNextSpawn();
        var bikeGo = GameObject.Instantiate(bikePrefab, spawn.position, spawn.rotation);
        var networkObject = bikeGo.GetComponent<NetworkObject>();
        networkObject.Spawn();
        caller.SentMessageClientRpc(playerIndex, networkObject.NetworkObjectId);
    }
    */
}
