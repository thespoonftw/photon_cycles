
using UnityEngine;

public class ServerPlayer   
{
    public int Id { get; private set; }

    public Color Color { get; private set; }


    public ServerPlayer(int id, Color color)
    {
        Id = id;
        Color = color;
    }
}
