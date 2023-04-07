
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class ServerPlayerManager   
{
    private readonly Dictionary<int, ServerPlayer> players = new();

    private List<Color> availableColors = new List<Color>()
    {
        Color.cyan,
        new Color(1, 0.5f, 0),
        Color.magenta,
        Color.green,
        Color.yellow,
        Color.red,
        Color.blue,
        new Color(0.5f, 0, 1)
    };

    private int playerIndex = 0;

    public ServerPlayer AddPlayer(IInputController controllerType, ulong clientId)
    {
        var color = availableColors[0];
        availableColors.Remove(color);
        playerIndex += 1;
        var player = new ServerPlayer(playerIndex, color);
        players.Add(playerIndex, player);
        Debug.Log("Adding player");
        return player;
    }

    public List<ServerPlayer> GetAllPlayers()
    {
        return players.Values.ToList();
    }
}
