using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.EnemyWaveManager.entryPatterns
{
    public class FromTopEntry : IEntry
    {
        private float targetY;

        public FromTopEntry(float targetY)
        {
            this.targetY = targetY;
        }

        public void Enter(Entity entity, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            entity.Position.Y += 150 * delta; // Slide down

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