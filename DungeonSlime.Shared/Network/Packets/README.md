# Packet System Usage Guide

This document explains how to use the automatic packet serialization system with NetCode.

## Overview

The packet system automatically writes and reads packet type identifiers, making it easy to serialize/deserialize packets over the network.

## Key Components

- **PacketType enum**: Defines all available packet types
- **ISerializable interface**: All packets must implement this interface
- **PacketWriter**: Handles writing packets with type information
- **PacketReader**: Handles reading and automatically deserializing packets

## Creating a New Packet

1. Add the packet type to the `PacketType` enum in `ISerializable.cs`:

```csharp
public enum PacketType : uint
{
    Unknown = 0,
    AddPlayerPacket = 1,
    RemovePlayerPacket = 2,
    YourNewPacket = 3,  // Add here
}
```

2. Create your packet class implementing `ISerializable`:

```csharp
public class YourNewPacket : ISerializable
{
    public PacketType Type => PacketType.YourNewPacket;
    
    public int SomeData;
    public string SomeString;
    
    // Parameterless constructor required for deserialization
    public YourNewPacket() { }
    
    public YourNewPacket(int data, string str)
    {
        SomeData = data;
        SomeString = str;
    }
    
    public void Serialize(BitWriter bitWriter)
    {
        bitWriter.Write(SomeData);
        bitWriter.Write(SomeString);
    }
    
    public void Deserialize(BitReader bitReader)
    {
        SomeData = bitReader.ReadInt();
        SomeString = bitReader.ReadString();
    }
}
```

3. Add the packet to the switch statement in `PacketReader.ReadPacket()`:

```csharp
ISerializable packet = packetType switch
{
    PacketType.AddPlayerPacket => new AddPlayerPacket(),
    PacketType.RemovePlayerPacket => new RemovePlayerPacket(),
    PacketType.YourNewPacket => new YourNewPacket(),  // Add here
    PacketType.Unknown => null,
    _ => null
};
```

## Usage Examples

### Sending a Packet

```csharp
// Create a BitWriter
var bitWriter = new BitWriter();

// Create your packet
var packet = new AddPlayerPacket(playerId: 12345, position: new Vector2(100, 200));

// Write the packet (automatically includes packet type)
PacketWriter.WritePacket(bitWriter, packet);

// Send the buffer over the network
byte[] buffer = bitWriter.GetBuffer();
// ... send buffer ...
```

### Receiving a Packet (Automatic Type Detection)

```csharp
// Receive data from network
byte[] buffer = // ... receive from network ...

// Create a BitReader
var bitReader = new BitReader(buffer);

// Read the packet (automatically detects type and deserializes)
ISerializable packet = PacketReader.ReadPacket(bitReader);

// Check what type of packet we received
if (packet is AddPlayerPacket addPlayerPacket)
{
    Console.WriteLine($"Player {addPlayerPacket.PlayerId} joined at {addPlayerPacket.Position}");
}
else if (packet is RemovePlayerPacket removePlayerPacket)
{
    Console.WriteLine($"Player {removePlayerPacket.playerId} left");
}
```

### Receiving a Packet (When You Know the Type)

```csharp
// If you already know what packet type to expect
byte[] buffer = // ... receive from network ...
var bitReader = new BitReader(buffer);

// Read and verify the packet type matches
var addPlayerPacket = PacketReader.ReadPacket<AddPlayerPacket>(bitReader);
Console.WriteLine($"Player {addPlayerPacket.PlayerId} joined");
```

### Just Reading the Packet Type

```csharp
// If you only want to peek at the packet type without deserializing
byte[] buffer = // ... receive from network ...
var bitReader = new BitReader(buffer);

PacketType type = PacketReader.ReadPacketType(bitReader);

if (type == PacketType.AddPlayerPacket)
{
    // Now continue reading the rest of the packet...
}
```

## Important Notes

1. **Parameterless Constructor**: All packet classes MUST have a parameterless constructor for deserialization to work
2. **Packet Type Order**: The packet type is ALWAYS written/read first
3. **Update the Switch**: When adding new packets, don't forget to update the switch statement in `PacketReader.ReadPacket()`
4. **Enum Values**: Use explicit uint values for `PacketType` to ensure network compatibility
