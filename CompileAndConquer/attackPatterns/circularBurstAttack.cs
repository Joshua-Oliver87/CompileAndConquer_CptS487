using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using System;


namespace CompileAndConquer.Content.AttackPatterns
{
    public class CircularBurstAttack : IAttackPattern
    {
        private float cooldown = 0f;
        private float cooldownDuration = 3f;

        private int bulletCount = 36;
        private float bulletSpeed = 200f;

        public CircularBurstAttack(int bulletCount = 36, float cooldown = 3f, float speed = 200f)
        {
            this.bulletCount = bulletCount;
            this.cooldownDuration = cooldown;
            this.bulletSpeed = speed;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (cooldown > 0) return;

            Vector2 center = enemy.Position;

            for (int i = 0; i < bulletCount; i++)
            {
                float angleDeg = i * (360f / bulletCount);
                float angleRad = MathHelper.ToRadians(angleDeg);

                Vector2 dir = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
                EntityHandler.Add(new EnemyBullet(center, new StraightBullet(dir, bulletSpeed)));
            }

            cooldown = cooldownDuration;
        }
    }
}