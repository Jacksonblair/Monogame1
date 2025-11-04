using System;
using System.Collections.Generic;
using DungeonSlime.Server.player;
using DungeonSlime.Shared;
using DungeonSlime.Shared.Network;
using Microsoft.Xna.Framework;
using NetCode;
using NetcodeIO.NET;

namespace DungeonSlime.Server;

public class GameServer : Core
{
    Dictionary<ulong, Player> players = [];
    NetcodeIO.NET.Server server;

    public GameServer()
        : base("Dungeon Slime", 1280, 720, false) { }

    protected override void Initialize()
    {
        base.Initialize();

        Window.Title = "Server";

        server = new NetcodeIO.NET.Server(
            999, // int maximum number of clients which can connect to this server at one time
            EnvSettings.LoadServerHost(),
            EnvSettings.LoadServerPort(), // string public address and int port clients will connect to
            EnvSettings.LoadProtocolId(), // ulong protocol ID shared between clients and server
            EnvSettings.LoadPrivateKey() // byte[32] private crypto key shared between backend servers
        );
        server.LogLevel = NetcodeLogLevel.Debug;
        server.Start(); // start the server running

        // Called when a client has connected
        server.OnClientConnected += clientConnectedHandler; // void( RemoteClient client )

        // Called when a client disconnects
        server.OnClientDisconnected += clientDisconnectedHandler; // void( RemoteClient client )

        // Called when a payload has been received from a client
        // Note that you should not keep a reference to the payload, as it will be returned to a pool after this call completes.
        server.OnClientMessageReceived += messageReceivedHandler; // void( RemoteClient client, byte[] payload, int payloadSize )
    }

    private void messageReceivedHandler(RemoteClient sender, byte[] payload, int payloadSize)
    {
        var bitReader = new BitReader(payload);
        PacketType type = PacketReader.ReadPacketType(bitReader);
        Console.WriteLine($"Received packet of type: {type}");

        switch (type)
        {
            case PacketType.PlayerMovementInputPacket:
                var upp_packet = new PlayerMovementInputPacket();
                upp_packet.Deserialize(bitReader);

                // Update player position in server state
                var gotPlayer = players.TryGetValue(sender.ClientID, out var player);
                if (gotPlayer)
                {
                    player.Position.X += upp_packet.Velocity.X;
                    player.Position.Y += upp_packet.Velocity.Y;
                }
                break;
            default:
                Console.WriteLine("DIDNT RECOGNISE PACKET TYPE");
                break;
        }
    }

    private void clientDisconnectedHandler(RemoteClient client)
    {
        players.Remove(client.ClientID);
        Console.WriteLine($"Client disconnected: {client.ClientID}");
        BroadcastPacket(new RemovePlayerPacket(client.ClientID));
    }

    private void clientConnectedHandler(RemoteClient client)
    {
        var netPlayer = new Player(client, Vector2.Zero);
        players.Add(client.ClientID, netPlayer);
        Console.WriteLine($"Client connected: {client.ClientID}");
        BroadcastPacket(new AddPlayerPacket(netPlayer.ToNetworkedPlayer()));
    }

    protected override void LoadContent()
    {
        // _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here

        // Broadcast position of all players
        foreach (var p in players)
        {
            var positionPacket = new UpdatePlayerPositionPacket();
            positionPacket.PlayerId = p.Value.Client.ClientID;
            positionPacket.Position = p.Value.Position;
            BroadcastPacket(positionPacket);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Add your drawing code here
        base.Draw(gameTime);
    }

    void BroadcastPacket(Shared.Network.ISerializable packet)
    {
        // Console.WriteLine("BROADCASTING PACKET");
        var writer = new BitWriter();
        PacketWriter.WritePacket(writer, packet);

        // Send packet to all players.
        foreach (var player in players.Values)
        {
            // Console.WriteLine(
            //     $"Sending to: {player.Client.RemoteEndpoint} {player.Client.ClientID}"
            // );
            player.Client.SendPayload(writer.Array, writer.BytesCount);
        }
    }
}
