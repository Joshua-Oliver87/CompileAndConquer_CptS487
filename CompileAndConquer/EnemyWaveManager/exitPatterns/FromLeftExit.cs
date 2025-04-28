using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.EnemyWaveManager.exitPatterns
{
    /// <summary>
    /// Sends the entity off to the left beyond targetX.
    /// </summary>
    public class FromLeftExit : IExit
    {
        private readonly float targetX;
        private readonly float speed;

        public FromLeftExit(float targetX, float speed = 100f)
        {
            this.targetX = targetX;
            this.speed   = speed;
        }

        public void Exit(Entity e, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            e.Position.X -= speed * dt;
            if (e.Position.X < targetX)
                e.Position.X = targetX;
        }
    }
}