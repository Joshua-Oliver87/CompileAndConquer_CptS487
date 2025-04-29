using Microsoft.Xna.Framework;
using CompileAndConquer.Content;                  // for Entity
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns; // for IExit

namespace CompileAndConquer.Content.EnemyWaveManager.exitPatterns
{
    /// <summary>
    /// Carries the entity from wherever it is now out to `Target` over `Duration`,
    /// along the same half‑parabolic arc (so they swoop off-screen).
    /// </summary>
    public class SwoopExit : IExit
    {
        private readonly Vector2 Target;
        private readonly float Duration;
        private readonly float Amplitude;
        private Vector2 Start;
        private float elapsed;

        public SwoopExit(Vector2 target, float duration = 1f, float amplitude = 150f)
        {
            Target    = target;
            Duration  = duration;
            Amplitude = amplitude;
        }

        public void Exit(Entity e, GameTime gameTime)
        {
            if (elapsed == 0f)
                Start = e.Position;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsed += dt;

            float t = MathHelper.Clamp(elapsed / Duration, 0, 1);

            float y = MathHelper.Lerp(Start.Y, Target.Y, t);
            float x = Start.X + Amplitude * t; // same monotonic rightward drift

            e.Position = new Vector2(x, y);
        }
    }
}