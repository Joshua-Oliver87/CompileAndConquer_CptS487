using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.EnemyWaveManager.exitPatterns
{
    public class fromTopExit : IExit
    {
        private float targetY;

        public fromTopExit(float targetY)
        {
            this.targetY = targetY;
        }

        public void Exit(Entity entity, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            entity.Position.Y -= 150 * delta; // Slide up

            if (entity.Position.Y >= targetY)
            {
                entity.Position.Y = targetY;
            }
        }
    }
}