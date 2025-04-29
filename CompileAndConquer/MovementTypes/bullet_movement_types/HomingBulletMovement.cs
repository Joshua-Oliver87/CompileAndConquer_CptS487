using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CompileAndConquer.Content.MovementTypes.BulletMovementTypes
{
    public class HomingBullet : IMovementStrategy
    {
        private readonly float maxLifetime = 2.5f; // seconds
        private readonly Dictionary<Entity, float> bulletTimers = new();

        public void Move(Entity bullet, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Track age of bullet
            if (!bulletTimers.ContainsKey(bullet))
                bulletTimers[bullet] = 0f;

            bulletTimers[bullet] += dt;

            if (bulletTimers[bullet] >= maxLifetime)
            {
                bullet.isAlive = false;
                bulletTimers.Remove(bullet); // cleanup
                return;
            }

            // Homing movement
            Vector2 target = Player.Instance.Position;
            Vector2 direction = target - bullet.Position;

            if (direction.LengthSquared() > 1f)
                direction.Normalize();

            float speed = 250f;
            bullet.Position += direction * speed * dt;

            //(fallback)
            if (bullet.Position.X < -bullet.Size.X || bullet.Position.X > CompileAndConquer.ScreenSize.X + bullet.Size.X ||
                bullet.Position.Y < -bullet.Size.Y || bullet.Position.Y > CompileAndConquer.ScreenSize.Y + bullet.Size.Y)
            {
                bullet.isAlive = false;
                bulletTimers.Remove(bullet);
            }
        }
    }
}