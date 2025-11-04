using System;
using Microsoft.Xna.Framework;
using NetCode;

namespace DungeonSlime.Shared.Network;

/** Server representation of player **/
public class AddPlayerPacket : ISerializable
{
    public PacketType Type => PacketType.AddPlayerPacket;

    public ulong PlayerId;
    public Vector2 Position;

    public AddPlayerPacket(NetworkedPlayer player)
    {
        PlayerId = player.Id;
        Position = player.Position;
    }

    public AddPlayerPacket(ulong playerId, Vector2 position)
    {
        PlayerId = playerId;
        Position = position;
    }

    public AddPlayerPacket() { }

    public void Serialize(BitWriter bitWriter)
    {
        Console.WriteLine($"Serializing: {PlayerId} {Position.X} {Position.Y}");
        bitWriter.Write(PlayerId);
        bitWriter.Write(Position.X);
        bitWriter.Write(Position.Y);
    }

    public void Deserialize(BitReader bitReader)
    {
        PlayerId = bitReader.ReadULong();
        Position = new Vector2();
        Position.X = bitReader.ReadFloat();
        Position.Y = bitReader.ReadFloat();
    }
}
