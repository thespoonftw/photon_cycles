using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ServerToClient : NetworkBehaviour
{
    public Action<INetworkSerializable> messageCallback;

    public void Init(Action<INetworkSerializable> messageCallback)
    {
        this.messageCallback = messageCallback;
    }

    [ClientRpc]
    public void ToClientRpc(INetworkSerializable message)
    {
        if (!IsOwner)
            return;

        messageCallback.Invoke(message);
        //var go = GetNetworkObject(objectId).gameObject;
    }
}
