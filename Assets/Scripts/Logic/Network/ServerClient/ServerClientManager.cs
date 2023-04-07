using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerClientManager
{
    public event Action OnChange;
    public event Action<ulong, INetworkSerializable> OnMessage;

    private Dictionary<ulong, ServerClient> clients;
    private readonly NetworkManager network;

    public ServerClientManager(NetworkManager network)
    {
        this.network = network;

        network.OnClientConnectedCallback += ClientConnected;
        network.OnClientDisconnectCallback += ClientDisconnected;
    }

    // TODO: call this
    public void Dispose()
    {
        network.OnClientConnectedCallback -= ClientConnected;
        network.OnClientDisconnectCallback -= ClientDisconnected;
    }

    public void ToClient(ulong clientID, INetworkSerializable message)
    {
        clients[clientID].ToClient.ToClientRpc(message);
    }

    private void ClientConnected(ulong clientId)
    {
        var toServer = network.ConnectedClients[clientId].PlayerObject.GetComponent<ClientToServer>();
        toServer.Init(MessageToServer);
        var toClient = network.ConnectedClients[clientId].PlayerObject.GetComponent<ServerToClient>();
        var client = new ServerClient(clientId, "New Client", toServer, toClient);

        clients.Add(clientId, client);
        Debug.Log(clientId + "connected");
        OnChange?.Invoke();
    }

    private void ClientDisconnected(ulong clientId)
    {
        clients.Remove(clientId);
        Debug.Log(clientId + "disconnected");
        OnChange?.Invoke();
    }

    private void MessageToServer(ulong clientId, INetworkSerializable message)
    {
        OnMessage.Invoke(clientId, message);
    }
}
