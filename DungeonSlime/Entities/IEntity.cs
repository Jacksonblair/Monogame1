using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;

public interface IEntity : ICollisionActor
{
    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch);
}
