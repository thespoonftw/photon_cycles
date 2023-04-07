using System;
using Unity.Netcode;

public class Client
{
    public event Action<INetworkSerializable> OnMessage;

    private readonly ClientToServer toServer;

    public Client(NetworkManager network)
    {
        toServer = network.LocalClient.PlayerObject.GetComponent<ClientToServer>();
        var toClient = network.LocalClient.PlayerObject.GetComponent<ServerToClient>();
        toClient.Init(ToClient);
    }

    public void ToServer(INetworkSerializable msg)
    {
        toServer.ToServerRpc(msg);
    }

    private void ToClient(INetworkSerializable msg)
    {
        OnMessage.Invoke(msg);
    }
}