using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class RepeatingSpiralAttack : IAttackPattern
    {
        private double elapsedTime = 0;
        private double spiralRate = 0.05; // time between shots

        private float angle = 0f;
        private float angleStep = 10f;

        private float baseBulletSpeed = 150f;
        private float speedGrowth = 2f; // speed added per bullet

        private int bulletsFired = 0;

        public RepeatingSpiralAttack(float angleStep = 10f, float baseBulletSpeed = 150f, float spiralRate = 0.05f, float speedGrowth = 2f)
        {
            this.angleStep = angleStep;
            this.baseBulletSpeed = baseBulletSpeed;
            this.spiralRate = spiralRate;
            this.speedGrowth = speedGrowth;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (enemy.BulletMovementStrategy == null) return;

            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime >= spiralRate)
            {
                float currentAngle = MathHelper.ToRadians(angle);
                Vector2 dir = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

                float spiralBulletSpeed = baseBulletSpeed + (bulletsFired * speedGrowth);

                var bullet = new EnemyBullet(enemy.Position, new StraightBullet(dir, spiralBulletSpeed));
                EntityHandler.Add(bullet);

                bulletsFired++;
                angle += angleStep;
                elapsedTime = 0;
            }
        }
    }
}