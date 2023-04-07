using Unity.Netcode;

public class Server
{
    public ServerPlayerManager Players { get; private set; }
    public ServerClientManager Clients { get; private set; }
    public LevelManager Levels { get; private set; }

    public Server(ServerPlayerManager players, LevelManager levels, ServerClientManager clients)
    {
        Players = players;
        Clients = clients;
        Levels = levels;
    }
}