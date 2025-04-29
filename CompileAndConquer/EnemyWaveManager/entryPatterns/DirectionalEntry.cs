using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Content.EnemyWaveManager;
using System;

namespace CompileAndConquer.Content.EnemyWaveManager.entryPatterns
{
    public class DirectionalEntry : IEntry
    {
        private readonly CardinalDirection dir;
        private readonly Vector2 target;
        private readonly float speed;
        private bool initialized;

        public DirectionalEntry(CardinalDirection dir, Vector2 target, float speed)
        {
            this.dir    = dir;
            this.target = target;
            this.speed  = speed;
        }

        public void Enter(Entity e, GameTime gameTime)
        {
            if (!initialized)
            {
                // place just off-screen
                var size = e.Size;
                var screen = CompileAndConquer.ScreenSize;
                Vector2 startOffset = dir switch
                {
                    CardinalDirection.Left   => new Vector2(-(screen.X + size.X),  0),
                    CardinalDirection.Right  => new Vector2( screen.X + size.X,   0),
                    CardinalDirection.Top    => new Vector2( 0, -(screen.Y + size.Y)),
                    CardinalDirection.Bottom => new Vector2( 0,  screen.Y + size.Y),
                    _ => Vector2.Zero
                };
                
                var start = target + startOffset;
                e.Position = start;
                initialized = true;
                //Console.WriteLine($"[Entry] {e.GetHashCode()} start at {e.Position}, target={target}");
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // move toward target
            var toTarget = target - e.Position;
            //Console.WriteLine($"[Entry] {e.GetHashCode()} at {e.Position}, toTarget.Len={toTarget.Length()}");
            if (toTarget != Vector2.Zero)
            {
                toTarget.Normalize();
                e.Position += toTarget * speed * dt;
            }
        }

        public bool HasEntered(Entity e)
        {
            switch(dir)
            {
                // “Spawn from left” → move right → done when X ≥ target.X
                case CardinalDirection.Left:   return e.Position.X >= target.X;

                // “Spawn from right” → move left → done when X ≤ target.X
                case CardinalDirection.Right:  return e.Position.X <= target.X;

                // “Spawn from top” → move down → done when Y ≥ target.Y
                case CardinalDirection.Top:    return e.Position.Y >= target.Y;

                // “Spawn from bottom” → move up → done when Y ≤ target.Y
                case CardinalDirection.Bottom: return e.Position.Y <= target.Y;

                default:                       return true;
            }
        }

    }
}