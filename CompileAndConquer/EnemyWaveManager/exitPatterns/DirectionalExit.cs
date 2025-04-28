using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Content.EnemyWaveManager;


namespace CompileAndConquer.Content.EnemyWaveManager.exitPatterns
{
    public class DirectionalExit : IExit
    {
        private readonly CardinalDirection dir;
        private readonly Vector2 target;
        private readonly float speed;

        public DirectionalExit(CardinalDirection dir, Vector2 target, float speed)
        {
            this.dir    = dir;
            this.target = target;
            this.speed  = speed;
        }

        public void Exit(Entity e, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var toTarget = target - e.Position;
            if (toTarget.LengthSquared() > 0.01f)
            {
                toTarget.Normalize();
                e.Position += toTarget * speed * dt;
            }
        }
    }    
}
