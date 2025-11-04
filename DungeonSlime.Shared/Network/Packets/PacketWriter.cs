using NetCode;

namespace DungeonSlime.Shared.Network;

public class PacketWriter
{
    /// <summary>
    /// Writes a packet with its type identifier to a BitWriter
    /// </summary>
    public static void WritePacket(BitWriter bitWriter, ISerializable packet)
    {
        // Write the packet type first
        bitWriter.Write((uint)packet.Type);

        // Then serialize the packet data
        packet.Serialize(bitWriter);

        // Flush
        bitWriter.Flush();
    }
}
