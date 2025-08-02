using System;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;

class NetworkedPlayerEntity : ICollisionActor
{
    private Player _data;
    private SpriteSheet _idleSpritesheet;
    private SpriteSheet _runSpritesheet;
    private RendererOne _renderer;
    private Vector2 _velocity;
    private AnimationController _idleAnimationController;
    private AnimationController _runAnimationController;

    private Vector2 _direction = Vector2.Zero;
    private Vector2 _lastDirection = Vector2.Zero;
    private bool _flip = false;
    private float _speed = 100;

    public Vector2 Position
    {
        get { return Bounds.Position; }
        // get { return _data.Position; }
    }

    public IShapeF Bounds { get; set; }

    public NetworkedPlayerEntity(Player data, RendererOne renderer, ContentManager contentMgr)
    {
        var idleTexture = contentMgr.Load<Texture2D>("images/noBKG_KnightIdle_strip");
        Texture2DAtlas atlas = Texture2DAtlas.Create("Atlas/Idle", idleTexture, 64, 64);
        _idleSpritesheet = new SpriteSheet("Idle", atlas);

        var runTexture = contentMgr.Load<Texture2D>("images/noBKG_KnightRun_strip");
        Texture2DAtlas runAtlas = Texture2DAtlas.Create("Run", runTexture, 96, 64);
        _runSpritesheet = new SpriteSheet("Run", runAtlas);

        _idleSpritesheet.DefineAnimation(
            "Idle",
            builder =>
            {
                builder
                    .IsLooping(true)
                    .AddFrame(regionIndex: 0, duration: TimeSpan.FromSeconds(0.1))
                    .AddFrame(1, TimeSpan.FromSeconds(0.1))
                    .AddFrame(2, TimeSpan.FromSeconds(0.1))
                    .AddFrame(3, TimeSpan.FromSeconds(0.1))
                    .AddFrame(4, TimeSpan.FromSeconds(0.1))
                    .AddFrame(5, TimeSpan.FromSeconds(0.1))
                    .AddFrame(6, TimeSpan.FromSeconds(0.1))
                    .AddFrame(7, TimeSpan.FromSeconds(0.1))
                    .AddFrame(8, TimeSpan.FromSeconds(0.1))
                    .AddFrame(9, TimeSpan.FromSeconds(0.1))
                    .AddFrame(10, TimeSpan.FromSeconds(0.1))
                    .AddFrame(11, TimeSpan.FromSeconds(0.1))
                    .AddFrame(12, TimeSpan.FromSeconds(0.1))
                    .AddFrame(13, TimeSpan.FromSeconds(0.1))
                    .AddFrame(14, TimeSpan.FromSeconds(0.1));
            }
        );

        _idleAnimationController = new AnimationController(_idleSpritesheet.GetAnimation("Idle"));

        _runSpritesheet.DefineAnimation(
            "Run",
            builder =>
            {
                builder
                    .IsLooping(true)
                    .AddFrame(regionIndex: 0, duration: TimeSpan.FromSeconds(0.1))
                    .AddFrame(1, TimeSpan.FromSeconds(0.1))
                    .AddFrame(2, TimeSpan.FromSeconds(0.1))
                    .AddFrame(3, TimeSpan.FromSeconds(0.1))
                    .AddFrame(4, TimeSpan.FromSeconds(0.1))
                    .AddFrame(5, TimeSpan.FromSeconds(0.1))
                    .AddFrame(6, TimeSpan.FromSeconds(0.1))
                    .AddFrame(7, TimeSpan.FromSeconds(0.1));
            }
        );
        _runAnimationController = new AnimationController(_runSpritesheet.GetAnimation("Run"));

        // TODO: Use animated sprite once ive got everything on one sheet.
        _data = data;

        // TODO: Tweak colission size getter thing?
        Bounds = new RectangleF(_data.Position, new SizeF(32, 32));

        _renderer = renderer;
    }

    public void Update(GameTime gameTime)
    {
        _lastDirection = _direction;
        _direction = GetMovementDirection();

        if (_direction != Vector2.Zero)
        {
            _flip = _direction.X > 0;
            _velocity = _direction.NormalizedCopy() * _speed;
            // _velocity = _direction.Normalize() * _speed;
            // Move(_direction, gameTime);
        }
        else
        {
            _velocity = Vector2.Zero;
        }

        MoveWithVelocity(gameTime);

        if (_direction == Vector2.Zero)
        {
            _idleAnimationController.Update(gameTime);
        }
        else
        {
            _runAnimationController.Update(gameTime);
        }
    }

    private void MoveWithVelocity(GameTime gameTime)
    {
        float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Bounds.Position += _velocity * deltaSeconds;
        // Position = actualPosition;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (_direction == Vector2.Zero)
        {
            var currentTexture = _idleSpritesheet.TextureAtlas[
                _idleAnimationController.CurrentFrame
            ];

            // _data.Tile = new Rectangle(0, 0, currentTexture.Width, currentTexture.Height);
            spriteBatch.Draw(
                currentTexture,
                Bounds.Position,
                Color.White,
                0.0f,
                _data.Pivot * new Vector2(currentTexture.Size.Width, currentTexture.Size.Height),
                Vector2.One,
                _flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                // SpriteEffects.None,
                0.0f
            );

            // WHat am i trying to do?
            // Make sure the sprite is drawn direclty on top of the position

            // spriteBatch.Draw(currentTexture, _idleSpritesheet.)
            // _renderer.RenderEntity(_data, currentTexture);
            // Console.WriteLine(_idleAnimationController.CurrentFrame);
        }
        else
        {
            var currentTexture = _runSpritesheet.TextureAtlas[_runAnimationController.CurrentFrame];
            spriteBatch.Draw(
                currentTexture,
                Bounds.Position,
                Color.White,
                0.0f,
                _data.Pivot * new Vector2(currentTexture.Size.Width, currentTexture.Size.Height),
                Vector2.One,
                _flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                // SpriteEffects.None,
                0.0f
            );

            // Console.WriteLine(_runAnimationController.CurrentFrame);
            // _data.Tile = new Rectangle(0, 0, currentTexture.Width, currentTexture.Height);
            // _renderer.RenderEntity(_data, currentTexture.Texture);
        }

        // Draw origin dot
        spriteBatch.DrawPoint(Bounds.Position, Color.Red, 5);
        spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Blue, 1);
    }

    private Vector2 GetMovementDirection()
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

    // public void Move(Vector2 direction, GameTime gameTime)
    // {
    //     float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    //     _data.Position += direction * _speed * deltaTime;
    //     // Console.WriteLine(_data.Position.ToString());
    // }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        Console.WriteLine("COLLIDED");
        _velocity.X *= -1;
        _velocity.Y *= -1;
        Bounds.Position -= collisionInfo.PenetrationVector;
        // _data.Position -= collisionInfo.PenetrationVector;
    }
}
