using Microsoft.Xna.Framework;

namespace CompileAndConquer.SpawnPatterns
{
    public interface ISpawnPattern
    {
        Vector2 GetPosition(int index, int total, Vector2 screenSize);
    }
}