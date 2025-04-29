using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class CircleAttack : IAttackPattern
    {
        private readonly int count;
        private readonly float radius;
        private readonly Func<Vector2, IMovementStrategy> movementFactory;
        private readonly float burstDuration;

        private float timer = 0f;
        private bool inBurst = true;
        private bool hasFired = false;

        private readonly Color color = Color.Yellow;

        public bool IsAttackInProgress => inBurst;

        public CircleAttack(
            int count,
            float radius,
            Func<Vector2, IMovementStrategy>? movementFactory = null,
            float burstDuration = 1.0f
        )
        {
            this.count = count;
            this.radius = radius;
            this.movementFactory = movementFactory ?? DefaultFactory;
            this.burstDuration = burstDuration;
            Reset();
        }

        public void Reset()
        {
            timer = 0f;
            inBurst = true;
            hasFired = false;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!inBurst) return;

            if (!hasFired)
            {
                Vector2 center = enemy.Position;
                Vector2 toPlayer = Player.Instance.Position - center;
                if (toPlayer != Vector2.Zero) toPlayer.Normalize();

                for (int i = 0; i < count; i++)
                {
                    float θ = MathHelper.TwoPi * i / count;
                    Vector2 offset = new Vector2((float)Math.Cos(θ), (float)Math.Sin(θ)) * radius;
                    Vector2 spawnPos = center + offset;

                    var move = movementFactory(toPlayer);
                    EntityHandler.Add(new EnemyBullet(spawnPos, move, color));
                }

                hasFired = true;
            }

            timer += dt;
            if (timer >= burstDuration)
                inBurst = false;
        }

        private IMovementStrategy DefaultFactory(Vector2 dir)
        {
            return new StraightBullet(dir, 200f);
        }
    }
}
