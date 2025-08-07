// using MemoryPack;
// using Microsoft.Xna.Framework;

// public class Vector2Formatter : MemoryPackFormatter<Vector2>
// {
//     public override void Serialize<TBufferWriter>(
//         ref MemoryPackWriter<TBufferWriter> writer,
//         scoped ref Vector2 value
//     )
//     {
//         writer.WriteValue(value.X);
//         writer.WriteValue(value.Y);
//     }

//     public override void Deserialize(ref MemoryPackReader reader, scoped ref Vector2 value)
//     {
//         value.X = reader.ReadValue(value.X);
//         value.Y = reader.ReadValue(value);
//     }
// }
