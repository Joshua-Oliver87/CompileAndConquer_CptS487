using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class TriangleAttack : IAttackPattern
    {
        private readonly int _rows;
        private readonly float _spacing;
        private readonly double _fireInterval;
        private float timer = 0f;
   		private bool hasFired = false;

        public bool IsAttackInProgress => false;

        public void Reset()
        {
            timer = 0f;
            hasFired = false;
        }

        private readonly Func<Vector2, IMovementStrategy> _bulletFactory;
        
        private double _timer;
        private Color color = Color.Yellow;

        public TriangleAttack(
            int rows,
            float spacing,
            double fireInterval,
            Func<Vector2, IMovementStrategy> bulletFactory
        ) {
            _rows           = rows;
            _spacing        = spacing;
            _fireInterval   = fireInterval;
            _bulletFactory  = bulletFactory;
            _timer          = fireInterval; // fire right away
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer < _fireInterval) return;
            _timer = 0;

            // center of the formation just below the enemy
            var spawnCenter = enemy.Position 
                            + new Vector2(0, enemy.Size.Y * 0.5f + _spacing);

            // everyone flies in the same direction
            Vector2 dir = Player.Instance.Position - spawnCenter;
            if (dir.LengthSquared() > 0) dir.Normalize();

            // vertical step for equilateral triangle
            float rowHeight = _spacing * (float)Math.Sqrt(3) / 2f;

            for (int row = 0; row < _rows; row++)
            for (int col = 0; col <= row; col++)
            {
                // two edges and the base
                bool isEdge = (col == 0 || col == row || row == _rows - 1);
                if (!isEdge) continue;

                float xOff = (col - row * 0.5f) * _spacing;
                float yOff = -row * rowHeight;
                var pos = spawnCenter + new Vector2(xOff, yOff);

                // ask factory for the right movement
                var move = _bulletFactory(dir);
                EntityHandler.Add(new EnemyBullet(pos, move, color));
            }
        }
    }
}
