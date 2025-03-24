using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.EnemyWaveManager.exitPatterns
{
    public class fromTopExit : IExit
    {
        private float targetY;
        private float speed;

        public fromTopExit(float targetY, float speed = 100f)
        {
            this.targetY = targetY;
            this.speed = speed;
        }

        public void Exit(Entity entity, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (entity.Position.Y > targetY)
            {
                entity.Position.Y -= speed * delta;

                // Clamp once passed
                if (entity.Position.Y < targetY)
                    entity.Position.Y = targetY;
            }
            else if (entity.Position.Y < targetY)
            {
                entity.Position.Y += speed * delta;

                if (entity.Position.Y > targetY)
                    entity.Position.Y = targetY;
            }
        }
    }

}