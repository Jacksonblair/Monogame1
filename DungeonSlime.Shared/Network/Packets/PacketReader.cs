using System;
using NetCode;

namespace DungeonSlime.Shared.Network;

public class PacketReader
{
    /// <summary>
    /// Reads the packet type from the BitReader and returns it
    /// </summary>
    public static PacketType ReadPacketType(BitReader bitReader)
    {
        uint packetTypeValue = bitReader.ReadUInt();
        return (PacketType)packetTypeValue;
    }

    /// <summary>
    /// Reads and deserializes a packet from the BitReader
    /// Returns the deserialized packet or null if the packet type is unknown
    /// </summary>
    public static ISerializable ReadPacket(BitReader bitReader)
    {
        // Read the packet type first
        PacketType packetType = ReadPacketType(bitReader);
        
        // Create the appropriate packet instance based on type
        ISerializable packet = packetType switch
        {
            PacketType.AddPlayerPacket => new AddPlayerPacket(),
            PacketType.RemovePlayerPacket => new RemovePlayerPacket(),
            PacketType.Unknown => null,
            _ => null
        };

        // Deserialize the packet data if we found a valid packet
        if (packet != null)
        {
            packet.Deserialize(bitReader);
        }

        return packet;
    }

    /// <summary>
    /// Reads and deserializes a packet of a specific type from the BitReader
    /// Use this when you already know the packet type
    /// </summary>
    public static T ReadPacket<T>(BitReader bitReader) where T : ISerializable, new()
    {
        // Read and verify packet type
        PacketType packetType = ReadPacketType(bitReader);
        
        T packet = new T();
        
        // Verify the packet type matches what we expect
        if (packet.Type != packetType)
        {
            throw new InvalidOperationException(
                $"Expected packet type {packet.Type} but got {packetType}");
        }
        
        packet.Deserialize(bitReader);
        return packet;
    }
}
