using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;

namespace CompileAndConquer.Content.EnemyWaveManager.entryPatterns
{
    /// <summary>
    /// Moves the entity from its current position into `Target` over `Duration` seconds,
    /// tracing a single swoop that drifts only to the right.
    /// </summary>
    public class SwoopEntry : IEntry
    {
        private readonly Vector2 Target;
        private readonly float Duration;
        private readonly float Amplitude;
        private Vector2 Start;
        private float elapsed;

        /// <param name="target">Where to end up vertically (X ignored).</param>
        /// <param name="duration">How many seconds the swoop takes.</param>
        /// <param name="amplitude">
        ///   How far right (pixels) to drift by the end of the swoop.
        /// </param>
        public SwoopEntry(Vector2 target, float duration = 1f, float amplitude = 150f)
        {
            Target    = target;
            Duration  = duration;
            Amplitude = amplitude;
        }

        public void Enter(Entity e, GameTime gameTime)
        {
            if (elapsed == 0f)
                Start = e.Position;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsed += dt;

            // t goes from 0 → 1 over Duration
            float t = MathHelper.Clamp(elapsed / Duration, 0, 1);

            // Vertical movement: Lerp from start Y to target Y
            float y = MathHelper.Lerp(Start.Y, Target.Y, t);

            // Horizontal drift: monotonic from Start.X to Start.X + Amplitude
            // (you can also use t*t for easing‑in)
            float eased = t < 0.5f
                ? 2*t*t
                : -1 + (4 - 2*t)*t;  // a simple ease in/out curve
            float x = Start.X + Amplitude * eased;

            e.Position = new Vector2(x, y);
        }

        public bool HasEntered(Entity e) => elapsed >= Duration;
    }
}