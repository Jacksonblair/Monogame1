using DungeonSlime.Shared.Network;
using Microsoft.Xna.Framework;
using NetCode;

namespace DungeonSlime.Tests
{
    public class AddPlayerPacketTests
    {
        [Fact]
        public void Serialize_Deserialize_WorksCorrectly()
        {
            ulong originalPlayerId = 123456789;
            Vector2 originalPosition = new Vector2(12.34f, 56.78f);
            var originalPacket = new AddPlayerPacket(originalPlayerId, originalPosition);

            var writer = new BitWriter();
            originalPacket.Serialize(writer);
            writer.Flush();
            byte[] buffer = writer.Array;

            var reader = new BitReader(buffer);
            var deserializedPacket = new AddPlayerPacket();
            deserializedPacket.Deserialize(reader);

            Assert.Equal(originalPacket.PlayerId, deserializedPacket.PlayerId);
            Assert.Equal(originalPacket.Position.X, deserializedPacket.Position.X);
            Assert.Equal(originalPacket.Position.Y, deserializedPacket.Position.Y);
        }
    }
}
