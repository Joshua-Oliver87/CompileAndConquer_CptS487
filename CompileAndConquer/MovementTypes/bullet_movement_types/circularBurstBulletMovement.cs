using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CompileAndConquer.Content.MovementTypes.BulletMovementTypes
{
    public class CircularBulletBurstMovement : IMovementStrategy
    {
        private bool hasFired = false;
        private int bulletCount;
        private float bulletSpeed;
		private Color color = Color.Yellow;

        public CircularBulletBurstMovement(int bulletCount = 12, float bulletSpeed = 200f)
        {
            this.bulletCount = bulletCount;
            this.bulletSpeed = bulletSpeed;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            if (hasFired) return;
            hasFired = true;

            float angleStep = MathHelper.TwoPi / bulletCount;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                IMovementStrategy bulletStrategy = new StraightBullet(dir, bulletSpeed);

                var burstBullet = new EnemyBullet(entity.Position, bulletStrategy, color);
                EntityHandler.Add(burstBullet);
            }

            // Remove the original "dummy" entity since it was just a burst trigger
            entity.isAlive = false;
        }
    }
}