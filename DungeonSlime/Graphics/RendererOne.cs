using LDtk;
using LDtk.Renderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

class RendererOne : ExampleRenderer
{
    public RendererOne(SpriteBatch spriteBatch)
        : base(spriteBatch) { }

    /// <summary> Renders the entity with the TextureRegion it includes. </summary>
    /// <param name="entity">The entity you want to render.</param>
    /// <param name="textRegion">The textureRegion for rendering the entity.</param>
    public void RenderEntity<T>(T entity, Texture2DRegion textureRegion)
        where T : ILDtkEntity
    {
        SpriteBatch.Draw(textureRegion, entity.Position, Color.White);
    }
}
