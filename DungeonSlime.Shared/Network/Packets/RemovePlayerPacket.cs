using NetCode;

namespace DungeonSlime.Shared.Network
{
    public class RemovePlayerPacket : ISerializable
    {
        public PacketType Type => PacketType.RemovePlayerPacket;

        public ulong PlayerId;

        public RemovePlayerPacket(ulong playerId)
        {
            this.PlayerId = playerId;
        }

        public RemovePlayerPacket() { }

        public void Serialize(BitWriter bitWriter)
        {
            bitWriter.Write(PlayerId);
        }

        public void Deserialize(BitReader bitReader)
        {
            PlayerId = bitReader.ReadULong();
        }
    }
}
