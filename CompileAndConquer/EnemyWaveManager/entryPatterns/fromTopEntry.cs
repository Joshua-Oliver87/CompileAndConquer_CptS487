using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.EnemyWaveManager.entryPatterns
{
    public class FromTopEntry : IEntry
    {
        private float targetY;
        private float speed; // pixels per second

        public FromTopEntry(float targetY, float speed = 50f) // Default slower speed
        {
            this.targetY = targetY;
            this.speed = speed;
        }

        public void Enter(Entity entity, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            entity.Position.Y += speed * delta;

            if (entity.Position.Y >= targetY)
            {
                entity.Position.Y = targetY;
            }
        }

        public bool HasEntered(Entity entity)
        {
            return entity.Position.Y >= targetY;
        }
    }
}