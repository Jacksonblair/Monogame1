using DungeonSlime.Shared.Network;
using Microsoft.Xna.Framework;
using NetcodeIO.NET;

namespace DungeonSlime.Server.player;

public class Player
{
    public Vector2 Position;
    public RemoteClient Client;

    public Player(RemoteClient client, Vector2 position)
    {
        Client = client;
        Position = position;
    }

    public NetworkedPlayer ToNetworkedPlayer()
    {
        return new NetworkedPlayer(Client.ClientID, Position);
    }
}
