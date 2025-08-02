using System;
using System.Collections;
using System.Collections.Generic;
using LDtk;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

public class IntGridTileIterator : IEnumerable<IntGridTile>
{
    private readonly LDtkIntGrid _grid;
    private readonly Vector2 _levelOffset;

    public IntGridTileIterator(LDtkIntGrid grid, Vector2 levelOffset)
    {
        _grid = grid;
        _levelOffset = levelOffset;
    }

    public IEnumerator<IntGridTile> GetEnumerator()
    {
        int gridWidth = _grid.GridSize.X;
        int tileSize = _grid.TileSize;

        for (int i = 0; i < _grid.Values.Length; i++)
        {
            int value = _grid.Values[i];

            int tileX = i % gridWidth;
            int tileY = i / gridWidth;

            float worldX = _levelOffset.X + tileX * tileSize;
            float worldY = _levelOffset.Y + tileY * tileSize;

            yield return new IntGridTile
            {
                GridX = tileX,
                GridY = tileY,
                WorldX = worldX,
                WorldY = worldY,
                Value = value
            };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public struct IntGridTile
{
    public int GridX;
    public int GridY;
    public float WorldX;
    public float WorldY;
    public int Value;
}
