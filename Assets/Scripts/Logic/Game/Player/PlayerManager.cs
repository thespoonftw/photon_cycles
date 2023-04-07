
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerManager   
{
    private readonly Dictionary<int, Player> players = new();

    public Player AddPlayer(int playerId, IInputController controllerType, Color color)
    {;
        var player = new Player(playerId, controllerType, color);
        players.Add(playerId, player);
        return player;
    }

    public List<Player> GetAllPlayers()
    {
        return players.Values.ToList();
    }
}
