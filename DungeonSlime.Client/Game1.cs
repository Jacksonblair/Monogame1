using System;
using System.Collections.Generic;
using DungeonSlime.Shared;
using LDtk;
// Optional
using LDtk.Renderer;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.Collisions.QuadTree;
using MonoGame.Extended.ViewportAdapters;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using NetcodeIO.NET;

namespace DungeonSlime;

public class Game1 : Core
{
    ProgramStartOptions startOpts;

    // Defines the slime animated sprite.
    // private AnimatedSprite _slime;

    // Defines the bat animated sprite.
    // private AnimatedSprite _bat;

    // Tracks the position of the slime.
    // private Vector2 _slimePosition;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    List<PlayerEntity> players = new List<PlayerEntity>();

    // Tracks the position of the bat.
    // private Vector2 _batPosition;

    // Tracks the velocity of the bat.
    // private Vector2 _batVelocity;

    // Defines the tilemap to draw.
    // private Tilemap _tilemap;

    // Defines the bounds of the room that the slime and bat are contained within.
    // private Rectangle _roomBounds;

    // Collision test
    private CollisionComponent _collisionComponent;

    LDtkFile LDtkFile;
    LDtkWorld World;
    RendererOne Renderer;
    List<Door> doors = new List<Door>();
    PlayerEntity player;

    // Extended camera
    OrthographicCamera _camera;
    List<CubeEntity> _cubeEntities = new List<CubeEntity>();

    // 1. Core systems in constructor
    public Game1(ProgramStartOptions startOpts)
        : base("Dungeon Slime", 1280, 720, false)
    {
        this.startOpts = startOpts;
    }

    // 2. Game specific initialisations in Initialize
    protected override void Initialize()
    {
        Server server = new Server(
            999, // int maximum number of clients which can connect to this server at one time
            startOpts.Host,
            startOpts.Port, // string public address and int port clients will connect to
            123, // ulong protocol ID shared between clients and server
            new byte[1234] // byte[32] private crypto key shared between backend servers
        );
        server.Start(); // start the server running

        // TODO: Add your initialization logic here

        base.Initialize(); // Should never be removed,  as this is where the graphics device is initialized for the target platform.

        LDtkFile = LDtkFile.FromFile(
            "C:/Users/Jackson/AppData/Local/Programs/ldtk/extraFiles/samples/Typical_TopDown_example_edited.ldtk"
        );
        World = LDtkFile.LoadWorld(Worlds.World.Iid);
        Renderer = new RendererOne(SpriteBatch);

        var worldBounds = LdtkWorldBoundsHelper.GetWorldBounds(World);

        // Level collision
        _collisionComponent = new CollisionComponent(
            new RectangleF(0, 0, worldBounds.Width, worldBounds.Height)
        );

        // Add layer for level collision blocks
        QuadTreeSpace quadTreeSpace = new QuadTreeSpace(
            new RectangleF(0, 0, worldBounds.Width, worldBounds.Height)
        );

        Layer myQuadLayer = new Layer(quadTreeSpace);
        _collisionComponent.Add("Collision", myQuadLayer);

        // Add all blocks from collision level
        foreach (LDtkLevel level in World.Levels)
        {
            Renderer.PrerenderLevel(level);
            LDtkIntGrid collisions = level.GetIntGrid("Collisions");
            var tileSize = collisions.TileSize;
            var intGridIterator = new IntGridTileIterator(collisions, level.Position.ToVector2());

            foreach (var tile in intGridIterator)
            {
                if (tile.Value > 0)
                {
                    var collider = new CubeEntity(
                        new RectangleF(
                            tile.WorldX,
                            tile.WorldY,
                            collisions.TileSize,
                            collisions.TileSize
                        ),
                        "Collision"
                    );

                    _cubeEntities.Add(collider);
                    _collisionComponent.Insert(collider);
                }
            }
        }

        var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
        _camera = new OrthographicCamera(viewportAdapter);

        // var playerData = World.GetEntity<Player>();
        // this.player = new PlayerEntity(playerData, Renderer, Content);

        // _collisionComponent.Insert(player);

        // Rectangle screenBounds = GraphicsDevice.PresentationParameters.Bounds;

        // _roomBounds = new Rectangle(
        //     (int)_tilemap.TileWidth,
        //     (int)_tilemap.TileHeight,
        //     screenBounds.Width - (int)_tilemap.TileWidth * 2,
        //     screenBounds.Height - (int)_tilemap.TileHeight * 2
        // );

        // // Initial slime position will be the center tile of the tile map.
        // int centerRow = _tilemap.Rows / 2;
        // int centerColumn = _tilemap.Columns / 2;
        // _slimePosition = new Vector2(
        //     centerColumn * _tilemap.TileWidth,
        //     centerRow * _tilemap.TileHeight
        // );

        // Initial bat position will be in the top left corner of the room
        // _batPosition = new Vector2(_roomBounds.Left, _roomBounds.Top);

        // Assign the initial random velocity to the bat.
        // AssignRandomBatVelocity();
    }

    // LoadContent is executed during the base.Initialize() method call within the Initialize method. It is important to know this because anything being initialized that is dependent on content loaded should be done after the base.Initialize() call and not before.
    protected override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
        // TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        // _slime = atlas.CreateAnimatedSprite("slime-animation");
        // _slime.Scale = new Vector2(4.0f, 4.0f);

        // Create the bat animated sprite from the atlas.
        // _bat = atlas.CreateAnimatedSprite("bat-animation");
        // _bat.Scale = new Vector2(4.0f, 4.0f);

        // Create the tilemap from the XML configuration file.
        // _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
        // _tilemap.Scale = new Vector2(4.0f, 4.0f);
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        // Check for keyboard input and handle it.
        // CheckKeyboardInput();

        // Check for gamepad input and handle it.
        // CheckGamePadInput();

        // _camera.Position = player.Position;
        // _camera.LookAt(player.Position);
        //  (
        //     GetCameraMovementDirection() * movementSpeed * gameTime.GetElapsedSeconds()
        // );
        // const float movementSpeed = 200;
        // AdjustCameraZoom();
        // player.Update(gameTime);




        _collisionComponent.Update(gameTime);
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

    private void AssignRandomBatVelocity()
    {
        // Generate a random angle
        // float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        // Convert angle to a direction vector
        // float x = (float)Math.Cos(angle);
        // float y = (float)Math.Sin(angle);
        // Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed
        // _batVelocity = direction * MOVEMENT_SPEED;
    }

    private void CheckKeyboardInput()
    {
        // If the space key is held down, the movement speed increases by 1.5
        // float speed = MOVEMENT_SPEED;
        // if (Input.Keyboard.IsKeyDown(Keys.Space))
        // {
        //     speed *= 1.5f;
        // }

        // // If the W or Up keys are down, move the slime up on the screen.
        // if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
        // {
        //     _slimePosition.Y -= speed;
        // }

        // // if the S or Down keys are down, move the slime down on the screen.
        // if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
        // {
        //     _slimePosition.Y += speed;
        // }

        // // If the A or Left keys are down, move the slime left on the screen.
        // if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
        // {
        //     _slimePosition.X -= speed;
        // }

        // // If the D or Right keys are down, move the slime right on the screen.
        // if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
        // {
        //     _slimePosition.X += speed;
        // }
    }

    private void CheckGamePadInput()
    {
        // GamePadInfo gamePadOne = Input.GamePads[(int)PlayerIndex.One];

        // // If the A button is held down, the movement speed increases by 1.5
        // // and the gamepad vibrates as feedback to the player.
        // float speed = MOVEMENT_SPEED;
        // if (gamePadOne.IsButtonDown(Buttons.A))
        // {
        //     speed *= 1.5f;
        //     gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(1));
        // }
        // else
        // {
        //     gamePadOne.StopVibration();
        // }

        // // Check thumbstick first since it has priority over which gamepad input
        // // is movement.  It has priority since the thumbstick values provide a
        // // more granular analog value that can be used for movement.
        // if (gamePadOne.LeftThumbStick != Vector2.Zero)
        // {
        //     _slimePosition.X += gamePadOne.LeftThumbStick.X * speed;
        //     _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * speed;
        // }
        // else
        // {
        //     // If DPadUp is down, move the slime up on the screen.
        //     if (gamePadOne.IsButtonDown(Buttons.DPadUp))
        //     {
        //         _slimePosition.Y -= speed;
        //     }

        //     // If DPadDown is down, move the slime down on the screen.
        //     if (gamePadOne.IsButtonDown(Buttons.DPadDown))
        //     {
        //         _slimePosition.Y += speed;
        //     }

        //     // If DPapLeft is down, move the slime left on the screen.
        //     if (gamePadOne.IsButtonDown(Buttons.DPadLeft))
        //     {
        //         _slimePosition.X -= speed;
        //     }

        //     // If DPadRight is down, move the slime right on the screen.
        //     if (gamePadOne.IsButtonDown(Buttons.DPadRight))
        //     {
        //         _slimePosition.X += speed;
        //     }
        // }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // // Begin the sprite batch to prepare for rendering.
        // SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // // Draw the tilemap.
        // // _tilemap.Draw(SpriteBatch);

        // // Draw the slime sprite.
        // // _slime.Draw(SpriteBatch, _slimePosition);

        // // Draw the bat sprite 10px to the right of the slime.
        // // _bat.Draw(SpriteBatch, _batPosition);

        // // Always end the sprite batch when finished.
        // SpriteBatch.End();

        // GraphicsDevice.Clear(World.BgColor);

        var transformMatrix = _camera.GetViewMatrix();
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);

        // _spriteBatch.Begin(transformMatrix: transformMatrix);
        // SpriteBatch.DrawRectangle(new RectangleF(250,250,50,50), Color.Black, 1f);
        // _spriteBatch.End();

        {
            foreach (LDtkLevel level in World.Levels)
            {
                Renderer.RenderPrerenderedLevel(level);
            }
        }

        player.Draw(SpriteBatch, gameTime);
        _cubeEntities.ForEach(ce => ce.Draw(SpriteBatch));

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
