using Microsoft.Xna.Framework;
using NetCode;

namespace DungeonSlime.Shared.Network
{
    public class UpdatePlayerPositionPacket : ISerializable
    {
        public PacketType Type => PacketType.UpdatePlayerPositionPacket;

        public ulong PlayerId;
        public Vector2 Position;

        public UpdatePlayerPositionPacket(ulong playerId, Vector2 position)
        {
            this.PlayerId = playerId;
            this.Position = position;
        }

        public UpdatePlayerPositionPacket() { }

        public void Serialize(BitWriter bitWriter)
        {
            bitWriter.Write(PlayerId);
            bitWriter.Write(Position.X);
            bitWriter.Write(Position.Y);
        }

        public void Deserialize(BitReader bitReader)
        {
            PlayerId = bitReader.ReadULong();
            var pos = new Vector2();
            pos.X = bitReader.ReadFloat();
            pos.Y = bitReader.ReadFloat();
            this.Position = pos;
        }
    }
}
