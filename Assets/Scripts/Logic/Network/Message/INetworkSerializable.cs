

using Unity.Netcode;
using UnityEngine;

public struct PlayerJoinedMsg : INetworkSerializable
{
    public int ControllerIndex { get; set; }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter { }
}

public struct PlayerJoinedSuccessMsg : INetworkSerializable
{
    public int PlayerId { get; set; }
    public int ControllerIndex { get; set; }
    public Color Color { get; set; }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter { }
}