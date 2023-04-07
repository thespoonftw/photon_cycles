using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ClientToServer : NetworkBehaviour
{
    private Action<ulong, INetworkSerializable> messageCallback;

    public void Init(Action<ulong, INetworkSerializable> messageCallback)
    {
        this.messageCallback = messageCallback;
    }

    [ServerRpc]
    public void ToServerRpc(INetworkSerializable message)
    {
        messageCallback.Invoke(OwnerClientId, message);
    }
}
