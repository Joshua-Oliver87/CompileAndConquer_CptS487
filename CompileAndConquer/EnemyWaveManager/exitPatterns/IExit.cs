using CompileAndConquer.Content;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content.EnemyWaveManager.exitPatterns
{
    public interface IExit
    {
        void Exit(Entity entity, GameTime gameTime);
    }
}