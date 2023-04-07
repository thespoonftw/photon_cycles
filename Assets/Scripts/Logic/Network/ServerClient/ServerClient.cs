
public class ServerClient   
{
    public string Name { get; private set; }

    public ulong Id { get; private set; }

    public ClientToServer ToServer { get; private set; }

    public ServerToClient ToClient { get; private set; }

    public ServerClient(ulong id, string name, ClientToServer toServer, ServerToClient toClient)
    {
        Id = id;
        Name = name;
        ToServer = toServer;
        ToClient = toClient;
    }
}
