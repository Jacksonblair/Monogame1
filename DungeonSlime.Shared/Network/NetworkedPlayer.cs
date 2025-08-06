using System;
using MessagePack;
using Microsoft.Xna.Framework;

namespace DungeonSlime.Shared.Network
{
    [MessagePackObject]
    public class NetworkedPlayer
    {
        public NetworkedPlayer(int id, Vector2 position)
        {
            this.Id = id;
            this.Position = position;
        }

        [Key(0)]
        public int Id;

        [Key(1)]
        public Vector2 Position;
    }
}
