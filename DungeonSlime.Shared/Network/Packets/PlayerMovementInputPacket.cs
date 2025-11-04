using Microsoft.Xna.Framework;
using NetCode;

namespace DungeonSlime.Shared.Network
{
    public class PlayerMovementInputPacket : ISerializable
    {
        public PacketType Type => PacketType.PlayerMovementInputPacket;
        public Vector2 Velocity;

        public PlayerMovementInputPacket(Vector2 velocity)
        {
            this.Velocity = velocity;
        }

        public PlayerMovementInputPacket() { }

        public void Serialize(BitWriter bitWriter)
        {
            bitWriter.Write(Velocity.X);
            bitWriter.Write(Velocity.Y);
        }

        public void Deserialize(BitReader bitReader)
        {
            var newVelocity = new Vector2();
            newVelocity.X = bitReader.ReadFloat();
            newVelocity.Y = bitReader.ReadFloat();
            this.Velocity = newVelocity;
        }
    }
}
