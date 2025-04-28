using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;

namespace CompileAndConquer.Content.EnemyWaveManager.entryPatterns
{
    /// <summary>
    /// Slides the entity in from the left edge to a target X.
    /// </summary>
    public class FromLeftEntry : IEntry
    {
        private readonly float targetX;
        private readonly float speed;

        public FromLeftEntry(float targetX, float speed = 100f)
        {
            this.targetX = targetX;
            this.speed   = speed;
        }

        public void Enter(Entity e, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            e.Position.X += speed * dt;
            if (e.Position.X >= targetX)
                e.Position.X = targetX;
        }

        public bool HasEntered(Entity e) => e.Position.X >= targetX;
    }
}