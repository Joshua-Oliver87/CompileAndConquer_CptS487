using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.Content.Interfaces;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class CircleAttack : IAttackPattern
    {
        private bool _hasFired;

        public bool IsAttackInProgress => !_hasFired;
        private readonly int _count;
        private readonly float _radius;
        private readonly Func<Vector2, IMovementStrategy> _movementFactory;
        private Color color = Color.Yellow;
		private float timer = 0f;
   		private bool hasFired = false;

        /// <param name="count">How many bullets around the circumference.</param>
        /// <param name="radius">Radius of the ring.</param>
        /// <param name="bulletSpeed">Speed at which the ring flies toward the player.</param>
        public CircleAttack(int count, float radius, Func<Vector2, IMovementStrategy> movementFactory)
        {
            _count = count;
            _radius = radius;
            _movementFactory = movementFactory;
        }

        public void Reset()
        {
            timer = 0f;
            hasFired = false;
        }


        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (_hasFired) return;

            Vector2 center = enemy.Position;
            Vector2 toPlayer = Player.Instance.Position - center;
            if (toPlayer != Vector2.Zero) toPlayer.Normalize();

            for (int i = 0; i < _count; i++)
            {
                float θ = MathHelper.TwoPi * i / _count;
                Vector2 offset = new Vector2((float)Math.Cos(θ), (float)Math.Sin(θ)) * _radius;
                Vector2 spawnPos = center + offset;

                var mv = _movementFactory(toPlayer);
                EntityHandler.Add(new EnemyBullet(spawnPos, mv, color));
            }

            _hasFired = true;
        }
    }
}