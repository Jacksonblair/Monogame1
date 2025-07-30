using System;
using System.Numerics;
using LDtk;
using MonoGame.Extended;

public static class LdtkWorldBoundsHelper
{
    /// <summary>
    /// Calculates the bounds of the entire LDtk world based on level positions and sizes.
    /// </summary>
    /// <param name="world">The LDtk world.</param>
    /// <returns>A RectangleF representing the total bounds of all levels.</returns>
    public static RectangleF GetWorldBounds(LDtkWorld world)
    {
        if (world.Levels == null || world.Levels.Length == 0)
            return RectangleF.Empty;

        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        foreach (var level in world.Levels)
        {
            var pos = level.Position;
            var size = level.Size;

            minX = MathF.Min(minX, pos.X);
            minY = MathF.Min(minY, pos.Y);
            maxX = MathF.Max(maxX, pos.X + size.X);
            maxY = MathF.Max(maxY, pos.Y + size.Y);
        }

        return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }
}
