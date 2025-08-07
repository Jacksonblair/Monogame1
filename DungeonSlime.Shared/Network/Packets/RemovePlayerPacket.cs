using NetCode;

namespace DungeonSlime.Shared.Network
{
    public class RemovePlayerPacket : ISerializable
    {
        public RemovePlayerPacket(ulong playerId)
        {
            this.playerId = playerId;
        }

        public ulong playerId;

        public void Serialize(BitWriter bitWriter)
        {
            throw new System.NotImplementedException();
        }

        public void Deserialize(BitReader bitReader)
        {
            throw new System.NotImplementedException();
        }
    }
}
