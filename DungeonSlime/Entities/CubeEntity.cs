using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class CubeEntity : ICollisionActor
{
    public Vector2 Velocity;
    public IShapeF Bounds { get; }
    public String LayerName { get; set; }

    public CubeEntity(RectangleF rectangleF, String layerName)
    {
        Bounds = rectangleF;
        LayerName = layerName;
    }

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        // Velocity.X *= -1;
        // Velocity.Y *= -1;
        // Bounds.Position -= collisionInfo.PenetrationVector;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 3);
    }
}
