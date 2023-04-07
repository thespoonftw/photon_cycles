using Unity.Netcode;

public class Game
{
    public MonoManager Mono { get; private set; }
    public NetworkManager Network { get; private set; }
    public InputManager Inputs { get; private set; }
    public ResourceManager Resources { get; private set; }
    public PlayerManager Players { get; private set; }
    public AccountManager Account { get; private set; }

    public Game(MonoManager mono, NetworkManager network, ResourceManager resources)
    {
        Mono = mono;
        Network = network;
        Resources = resources;
        Inputs = new();
        Players = new();
        Account = new();
    }
}