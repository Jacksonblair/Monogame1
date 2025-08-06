using System;
using System.Collections.Generic;
using DungeonSlime.Shared;
using DungeonSlime.Shared.Network;
using MessagePack;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetcodeIO.NET;

namespace DungeonSlime.Server;

public class GameServer : Core
{
    List<PlayerEntity> players = new List<PlayerEntity>();
    private int logMessageHandler;
    NetcodeIO.NET.Server server;

    public GameServer()
        : base("Dungeon Slime", 1280, 720, false) { }

    protected override void Initialize()
    {
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

        // Called when the server logs a message
        // If you are not using a custom logger, a handler using Console.Write() is sufficient.
        // server.OnLog += logMessageHandler; // void( string message, NetcodeLogLevel logLevel )

        // TODO: Add your initialization logic here

        base.Initialize();
    }

    private void messageReceivedHandler(RemoteClient sender, byte[] payload, int payloadSize)
    {
        NetworkedPlayer player = MessagePackSerializer.Deserialize<NetworkedPlayer>(payload);
        Console.WriteLine(player.ToString());
        Console.WriteLine($"Received Packet: {MessagePackSerializer.ConvertToJson(payload)}");

        sender.SendPayload(payload, payloadSize);
    }

    private void clientDisconnectedHandler(RemoteClient client)
    {
        Console.WriteLine(client.ToString());
    }

    private void clientConnectedHandler(RemoteClient client)
    {
        Console.WriteLine(client.ToString());
    }

    protected override void LoadContent()
    {
        // _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // GraphicsDevice.Clear(Color.CornflowerBlue);
        // TODO: Add your drawing code here
        base.Draw(gameTime);
    }
}
