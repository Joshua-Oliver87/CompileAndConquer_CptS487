using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Content.EnemyWaveManager;


namespace CompileAndConquer.Content.EnemyWaveManager.exitPatterns
{
    public class DirectionalExit : IExit
    {
        private readonly CardinalDirection dir;
        private readonly float speed;
        private Vector2 target;
        private bool    initialized;

        public DirectionalExit(CardinalDirection dir, float speed)
        {
            this.dir   = dir;
            this.speed = speed;
        }

        public void Exit(Entity e, GameTime gameTime)
        {
            // on first frame, compute our off‐screen target
            if (!initialized)
            {
                initialized = true;
                var screen = CompileAndConquer.ScreenSize;
                var size   = e.Size;
                switch (dir)
                {
                    case CardinalDirection.Left:
                        target = new Vector2(-size.X, e.Position.Y);
                        break;
                    case CardinalDirection.Right:
                        target = new Vector2(screen.X + size.X, e.Position.Y);
                        break;
                    case CardinalDirection.Top:
                        target = new Vector2(e.Position.X, -size.Y);
                        break;
                    case CardinalDirection.Bottom:
                        target = new Vector2(e.Position.X, screen.Y + size.Y);
                        break;
                }
            }

            // move toward it
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var to = target - e.Position;
            if (to != Vector2.Zero)
            {
                to.Normalize();
                e.Position += to * speed * dt;
            }

            // axis‐precise exit test
            bool done = dir switch
            {
                CardinalDirection.Left   => e.Position.X <= target.X,
                CardinalDirection.Right  => e.Position.X >= target.X,
                CardinalDirection.Top    => e.Position.Y <= target.Y,
                CardinalDirection.Bottom => e.Position.Y >= target.Y,
                _                        => (target - e.Position).LengthSquared() < 1f
            };
            if (done) e.isAlive = false;
        }
    }
}
