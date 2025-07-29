using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using LDtk.Renderer;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;

enum PlayerAnimationState
{
    Idle,
    Run
}

class PlayerEntity
{
    private Player _data;
    private SpriteSheet _idleSpritesheet;
    private SpriteSheet _runSpritesheet;
    private RendererOne _renderer;
    private AnimationController _idleAnimationController;
    private AnimationController _runAnimationController;

    public PlayerEntity(Player data, RendererOne renderer, ContentManager contentMgr)
    {
        var idleTexture = contentMgr.Load<Texture2D>("images/noBKG_KnightIdle_strip");
        Texture2DAtlas atlas = Texture2DAtlas.Create("Atlas/Idle", idleTexture, 144, 64);
        _idleSpritesheet = new SpriteSheet("Idle", atlas);

        var runTexture = contentMgr.Load<Texture2D>("images/noBKG_KnightRun_strip");
        Texture2DAtlas runAtlas = Texture2DAtlas.Create("Run", runTexture, 1, 1);
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

        _data = data;
        _renderer = renderer;
        _data.Tile = new Rectangle(0, 0, 64, 144);
    }

    public void Update(GameTime gameTime)
    {
        _idleAnimationController.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        var currentTexture = _idleSpritesheet.TextureAtlas[_idleAnimationController.CurrentFrame];
        this._renderer.RenderEntity(_data, currentTexture);
    }
}
