using System;
using System.Collections.Generic;
using DungeonSlime.Shared;
using DungeonSlime.Shared.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using NetCode;
using NetcodeIO.NET;

namespace DungeonSlime;

public class Game1 : Core
{
    Client client;
    ClientState clientState = ClientState.Disconnected;
    byte[] clientToken;
    ulong clientId;
    ClientStartOptions startOpts;
    ConnectTokenGetter connectTokenGetter;

    // Texture stuff?
    // TextureAtlas textureAtlas;

    // Speed multiplier when moving.
    // private const float MOVEMENT_SPEED = 5.0f;

    // Defines the tilemap to draw.
    // private Tilemap _tilemap;

    // Defines the bounds of the room that the slime and bat are contained within.
    // private Rectangle _roomBounds;

    // Collision test
    // private CollisionComponent _collisionComponent;

    // LDtkFile LDtkFile;
    // LDtkWorld World;
    RendererOne Renderer;

    // Extended camera
    OrthographicCamera _camera;

    // List<CubeEntity> _cubeEntities = new List<CubeEntity>();

    // Local player
    PlayerEntity player;

    // Networked players
    List<NetworkedPlayerEntity> _players = [];

    // 1. Core systems in constructor
    public Game1(ClientStartOptions startOpts)
        : base("Dungeon Slime", 1280, 720, false)
    {
        this.startOpts = startOpts;
    }

    // 2. Game specific initialisations in Initialize
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize(); // Should never be removed,  as this is where the graphics device is initialized for the target platform.

        Window.Title = "Client";

        // Network client
        client = new Client();

        // Called when the client's state has changed
        // Use this to detect when a client has connected to a server, or has been disconnected from a server, or connection times out, etc.
        client.OnStateChanged += ClientStateChanged; // void( ClientState state )

        // Called when a payload has been received from the server
        // Note that you should not keep a reference to the payload, as it will be returned to a pool after this call completes.
        client.OnMessageReceived += MessageReceivedHandler; // void( byte[] payload, int payloadSize )

        connectTokenGetter = new ConnectTokenGetter();
        connectTokenGetter
            .GetConnectTokenAsync()
            .ContinueWith(t =>
            {
                clientToken = t.Result.Token;
                clientId = t.Result.ClientId;
                Console.WriteLine($"Got connect token: {clientToken}");
                // Join server
                client.Connect(clientToken);
            });

        // Setup renderer...
        Renderer = new RendererOne(SpriteBatch);

        // LDtkFile = LDtkFile.FromFile(
        //     "C:/Users/Jackson/AppData/Local/Programs/ldtk/extraFiles/samples/Typical_TopDown_example_edited.ldtk"
        // );
        // World = LDtkFile.LoadWorld(Worlds.World.Iid);

        // var worldBounds = LdtkWorldBoundsHelper.GetWorldBounds(World);

        // // Level collision
        // _collisionComponent = new CollisionComponent(
        //     new RectangleF(0, 0, worldBounds.Width, worldBounds.Height)
        // );

        // // Add layer for level collision blocks
        // QuadTreeSpace quadTreeSpace = new QuadTreeSpace(
        //     new RectangleF(0, 0, worldBounds.Width, worldBounds.Height)
        // );

        // Layer myQuadLayer = new Layer(quadTreeSpace);
        // _collisionComponent.Add("Collision", myQuadLayer);

        // // Add all blocks from collision level
        // foreach (LDtkLevel level in World.Levels)
        // {
        //     Renderer.PrerenderLevel(level);
        //     LDtkIntGrid collisions = level.GetIntGrid("Collisions");
        //     var tileSize = collisions.TileSize;
        //     var intGridIterator = new IntGridTileIterator(collisions, level.Position.ToVector2());

        //     foreach (var tile in intGridIterator)
        //     {
        //         if (tile.Value > 0)
        //         {
        //             var collider = new CubeEntity(
        //                 new RectangleF(
        //                     tile.WorldX,
        //                     tile.WorldY,
        //                     collisions.TileSize,
        //                     collisions.TileSize
        //                 ),
        //                 "Collision"
        //             );

        //             _cubeEntities.Add(collider);
        //             _collisionComponent.Insert(collider);
        //         }
        //     }
        // }

        var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
        _camera = new OrthographicCamera(viewportAdapter);
    }

    private void MessageReceivedHandler(byte[] payload, int payloadSize)
    {
        Console.WriteLine($"Received message with size: {payloadSize}");

        BitReader bitReader = new BitReader(payload);
        PacketType type = PacketReader.ReadPacketType(bitReader);

        switch (type)
        {
            case PacketType.AddPlayerPacket:
                var ap_packet = new AddPlayerPacket();
                ap_packet.Deserialize(bitReader);
                Console.WriteLine(
                    $"GOT ADD PLAYER PACKET: {ap_packet.PlayerId} {ap_packet.Position.X} {ap_packet.Position.Y}"
                );

                var newPlayer = new NetworkedPlayerEntity(
                    ap_packet.PlayerId,
                    ap_packet.Position,
                    Renderer,
                    Content
                );
                _players.Add(newPlayer);
                break;
            case PacketType.RemovePlayerPacket:
                var rp_packet = new RemovePlayerPacket();
                rp_packet.Deserialize(bitReader);
                Console.WriteLine($"GOT REMOVE PLAYER PACKET: {rp_packet.PlayerId}");
                _players.RemoveAll(player => player.PlayerId == rp_packet.PlayerId);
                break;
            case PacketType.UpdatePlayerPositionPacket:
                var upp_packet = new UpdatePlayerPositionPacket();
                upp_packet.Deserialize(bitReader);
                // Console.WriteLine($"GOT UPDATE PLAYER POSITION PACKET: {upp_packet.PlayerId}");
                var playerToUpdate = _players.Find(p => p.PlayerId == upp_packet.PlayerId);
                if (playerToUpdate != null)
                {
                    // Console.WriteLine("FOUND PLAYER TO UPDATE");
                    var updatedPosition = new Vector2(upp_packet.Position.X, upp_packet.Position.Y);
                    playerToUpdate.Bounds.Position = updatedPosition;
                }
                break;
            default:
                Console.WriteLine("DIDNT RECOGNISE PACKET TYPE");
                break;
        }
    }

    private void ClientStateChanged(ClientState state)
    {
        clientState = state;
        Console.WriteLine($"Client state changed to: {clientState}");
        if (clientState == ClientState.Connected)
        {
            // var p = new NetworkedPlayer(1, Vector2.Zero);
            // var bytes = MessagePackSerializer.Serialize(p);
            // Console.WriteLine($"Sending packet: {MessagePackSerializer.ConvertToJson(bytes)}");
            // var bytes = new byte[2];
            // client.Send(bytes, 2);

            // Set up player entity on connect
            player = new(0, Vector2.Zero, Renderer, Content);
        }
    }

    // LoadContent is executed during the base.Initialize() method call within the Initialize method. It is important to know this because anything being initialized that is dependent on content loaded should be done after the base.Initialize() call and not before.

    protected override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        _camera.Position = player.Position;
        _camera.LookAt(player.Position);
        //  (
        //     GetCameraMovementDirection() * movementSpeed * gameTime.GetElapsedSeconds()
        // );
        // const float movementSpeed = 200;
        // AdjustCameraZoom();
        // player.Update(gameTime);

        if (client.State == ClientState.Connected)
        {
            // TODO: Move into player? OR somewhere else. idk.
            SendPacketToServer(new PlayerMovementInputPacket(player.Velocity));
        }

        if (player != null)
        {
            player.Update(gameTime);
        }
        _players.ForEach(p => p.Update(gameTime));

        // _collisionComponent.Update(gameTime);
        base.Update(gameTime);
    }

    private Vector2 GetCameraMovementDirection()
    {
        var movementDirection = Vector2.Zero;
        var state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.Down))
        {
            movementDirection += Vector2.UnitY;
        }
        if (state.IsKeyDown(Keys.Up))
        {
            movementDirection -= Vector2.UnitY;
        }
        if (state.IsKeyDown(Keys.Left))
        {
            movementDirection -= Vector2.UnitX;
        }
        if (state.IsKeyDown(Keys.Right))
        {
            movementDirection += Vector2.UnitX;
        }
        return movementDirection;
    }

    // Add this to the Game1.cs file
    private void AdjustCameraZoom()
    {
        var state = Keyboard.GetState();
        float zoomPerTick = 0.01f;
        if (state.IsKeyDown(Keys.Z))
        {
            _camera.ZoomIn(zoomPerTick);
        }
        if (state.IsKeyDown(Keys.X))
        {
            _camera.ZoomOut(zoomPerTick);
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        var mouseState = Mouse.GetState();
        return _camera.ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        var transformMatrix = _camera.GetViewMatrix();
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);

        // _spriteBatch.Begin(transformMatrix: transformMatrix);
        // SpriteBatch.DrawRectangle(new RectangleF(250,250,50,50), Color.Black, 1f);
        // _spriteBatch.End();

        // {
        //     foreach (LDtkLevel level in World.Levels)
        //     {
        //         Renderer.RenderPrerenderedLevel(level);
        //     }
        // }

        // this.player.Draw(SpriteBatch, gameTime);
        // _cubeEntities.ForEach(ce => ce.Draw(SpriteBatch));

        // Draw player, and all networked players.

        if (player != null)
        {
            player.Draw(SpriteBatch, gameTime);
        }
        _players.ForEach(p =>
        {
            p.Draw(SpriteBatch, gameTime);
        });

        SpriteBatch.End();

        base.Draw(gameTime);
    }

    void SendPacketToServer(ISerializable packet)
    {
        var writer = new BitWriter();
        PacketWriter.WritePacket(writer, packet);
        client.Send(writer.Array, writer.BytesCount);
    }
}
