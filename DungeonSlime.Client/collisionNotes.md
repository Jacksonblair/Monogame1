# LDtk Collision Integration Checklist (MonoGame.Extended)

Use this checklist to correctly generate colliders from LDtk IntGrid data and integrate them with MonoGame.Extended's `CollisionComponent`.

---

## 📦 Level Parsing
- [ ] Loop through each `LDtkLevel` in your world (`World.Levels`)
- [ ] Use `level.Position` (or `level.WorldPosition`) to get the level's top-left **world offset**
- [ ] Pre-render or load the level’s visuals if needed (`Renderer.PrerenderLevel(level)`)

---

## 🧱 IntGrid Setup
- [ ] Retrieve the `LDtkIntGrid` for collisions (e.g. `"Collisions"`)
- [ ] Read `TileSize` (in pixels) from the grid
- [ ] Use `GridSize.X` and `GridSize.Y` for grid width and height (in tiles)

---

## 🔲 Collider Creation
- [ ] Loop through `collisions.Values[]`
- [ ] For each `val > 0`:
  - [ ] Compute tile coordinates:
    - `x = i % GridSize.X`
    - `y = i / GridSize.X`
  - [ ] Convert tile to world space:
    - `worldX = level.Position.X + x * tileSize`
    - `worldY = level.Position.Y + y * tileSize`
  - [ ] Create a `RectangleF(worldX, worldY, tileSize, tileSize)`
  - [ ] Create a `CubeEntity` with that rectangle
  - [ ] Add to `_cubeEntities` (optional for debug/draw)
  - [ ] Insert into `_collisionComponent`

---

## 🎨 Debug Drawing (Optional)
- [ ] Add `CubeEntity.Draw(SpriteBatch)` to draw `Bounds` using `DrawRectangle`
- [ ] Use a transparent color like `Color.Red * 0.5f` for visual alignment with tilemap

---

## 🧠 CollisionComponent Tips
- [ ] Only insert **dynamic** entities (e.g. player, enemies) into `CollisionComponent.Update()`
- [ ] Static tile colliders should not be updated per frame
- [ ] Confirm that `CollisionComponent` uses spatial partitioning (QuadTree) to keep broad-phase fast

---

## ✅ Best Practices
- [ ] Always multiply grid indices by tile size to convert to world space
- [ ] Always offset by `level.Position` to get global position
- [ ] Ensure tile size matches rendering scale
- [ ] Profile performance if inserting 1000s of colliders (consider merging or chunking)

---

*Updated: July 2025 — based on MonoGame.Extended and LDtk integration*
