using Microsoft.Xna.Framework;

namespace DungeonSlime.Shared.Network
{
    public class NetworkedPlayer
    {
        public NetworkedPlayer(ulong id, Vector2 position)
        {
            this.Id = id;
            this.Position = position;
        }

        public ulong Id;

        public Vector2 Position;
    }
}
