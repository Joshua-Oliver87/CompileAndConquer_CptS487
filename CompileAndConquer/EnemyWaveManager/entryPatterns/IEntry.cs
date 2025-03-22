using CompileAndConquer.Content;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content.EnemyWaveManager.entryPatterns
{
    public interface IEntry
    {
        void Enter(Entity entity, GameTime gameTime);
        bool HasEntered(Entity entity);
    }
}