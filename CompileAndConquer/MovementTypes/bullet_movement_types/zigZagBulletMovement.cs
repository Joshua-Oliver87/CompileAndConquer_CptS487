using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using System;

namespace CompileAndConquer.Content.MovementTypes.BulletMovementTypes
{
    public class ZigZagBullet : IMovementStrategy
    {
        private readonly Vector2 _direction;
        private readonly Vector2 _perp;
        private readonly float   _speed;
        private readonly float   _amplitude;
        private readonly float   _frequency;
        private readonly float   _maxLife;
        private float            _age;
        
        public ZigZagBullet(
            Vector2 direction,
            float speed,
            float amplitude,
            float frequency,
            float maxLife = 5f
        ) {
            _direction  = direction;
            _perp       = new Vector2(-direction.Y, direction.X); // 90 degrees rotated
            _speed      = speed;
            _amplitude  = amplitude;
            _frequency  = frequency;
            _maxLife    = maxLife;
            _age        = 0f;
        }

        public void Move(Entity bullet, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _age += dt;
            if (_age >= _maxLife)
            {
                bullet.isAlive = false;
                return;
            }

            // forward step
            Vector2 forward = _direction * _speed * dt;
            float offset = _amplitude * (float)Math.Sin(MathHelper.TwoPi * _frequency * _age);
            Vector2 sideways = _perp * offset;

            bullet.Position += forward + sideways;
        }
    }
}
