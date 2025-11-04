using NetCode;

namespace DungeonSlime.Shared.Network;

public enum PacketType : uint
{
    Unknown = 0,
    AddPlayerPacket = 1,
    RemovePlayerPacket = 2,
    UpdatePlayerPositionPacket = 3,
    PlayerMovementInputPacket
}

public interface ISerializable
{
    PacketType Type { get; }
    public abstract void Serialize(BitWriter bitWriter);
    public abstract void Deserialize(BitReader bitReader);
}
