using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class TriangleAttack : IAttackPattern
    {
        private readonly int _rows;
        private readonly float _spacing;
        private readonly float _fireInterval;
        private float timer;

        private bool isFiring;

        private readonly Func<Vector2, IMovementStrategy> bulletFactory;
        private readonly Color color = Color.Yellow;

        public bool IsAttackInProgress => isFiring;

        public TriangleAttack(
            int rows,
            float spacing,
            float fireInterval = 2.0f,
            Func<Vector2, IMovementStrategy>? bulletFactory = null
        )
        {
            _rows = rows;
            _spacing = spacing;
            _fireInterval = fireInterval;
            this.bulletFactory = bulletFactory ?? DefaultBulletFactory;
            Reset();
        }

        public void Reset()
        {
            timer = _fireInterval; // fire right away
            isFiring = false;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer < _fireInterval)
                return;

            timer = 0f;
            isFiring = true;

            // Determine spawn center slightly below enemy
            var spawnCenter = enemy.Position + new Vector2(0, enemy.Size.Y * 0.5f + _spacing);
            Vector2 dir = Player.Instance.Position - spawnCenter;
            if (dir.LengthSquared() > 0) dir.Normalize();

            float rowHeight = _spacing * (float)Math.Sqrt(3) / 2f;

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col <= row; col++)
                {
                    // fire only edges and base of triangle
                    bool isEdge = (col == 0 || col == row || row == _rows - 1);
                    if (!isEdge) continue;

                    float xOffset = (col - row * 0.5f) * _spacing;
                    float yOffset = -row * rowHeight;
                    Vector2 spawnPos = spawnCenter + new Vector2(xOffset, yOffset);

                    var move = bulletFactory(dir);
                    EntityHandler.Add(new EnemyBullet(spawnPos, move, color));
                }
            }

            isFiring = false; // done firing this burst
        }

        private IMovementStrategy DefaultBulletFactory(Vector2 direction)
        {
            return new StraightBullet(direction, speed: 250f);
        }
    }
}
