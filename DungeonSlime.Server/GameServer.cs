using System.Collections.Generic;
using DungeonSlime.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetcodeIO.NET;

namespace DungeonSlime.Server;

public class GameServer : Core
{
    ServerStartOptions startOpts;
    List<PlayerEntity> players = new List<PlayerEntity>();

    public GameServer(ServerStartOptions startOpts)
        : base("Dungeon Slime", 1280, 720, false)
    {
        this.startOpts = startOpts;
    }

    protected override void Initialize()
    {
        NetcodeIO.NET.Server server = new NetcodeIO.NET.Server(
            999, // int maximum number of clients which can connect to this server at one time
            startOpts.Host,
            startOpts.Port, // string public address and int port clients will connect to
            123, // ulong protocol ID shared between clients and server
            new byte[1234] // byte[32] private crypto key shared between backend servers
        );
        server.LogLevel = NetcodeLogLevel.Debug;
        server.Start(); // start the server running
        // TODO: Add your initialization logic here

        base.Initialize();
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
