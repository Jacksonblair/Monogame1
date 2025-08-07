using NetCode;

namespace DungeonSlime.Shared.Network;

public enum PacketType : uint
{
    Unknown = 0,
    AddPlayerPacket,
    RemovePlayerPacket,
}

public interface ISerializable
{
    public abstract void Serialize(BitWriter bitWriter);
    public abstract void Deserialize(BitReader bitReader);
}
