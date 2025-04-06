using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class RepeatingSpiralAttack : IAttackPattern
    {
        private double elapsedTime = 0;
        private double spiralRate = 0.05;

        private int bulletIndex = 0;
        private int bulletDirections = 16;
        private float bulletAngleOffset;

        private float bulletCreationDistance = 20f;
        private float spiralHeight = 0f;
        private float spiralHeightStep = 4f;

        private float burstDuration = 1.6f;
        private float pauseDuration = 1.4f;
        private float cycleTimer = 0;
        private bool inBurst = true;

        private float bulletSpeed = 60f; // much slower for tight spiral

        public RepeatingSpiralAttack(
            int bulletDirections = 16,
            double spiralRate = 0.05,
            float bulletCreationDistance = 20f,
            float bulletSpeed = 60f,
            float burstDuration = 1.6f,
            float pauseDuration = 1.4f)
        {
            this.bulletDirections = bulletDirections;
            this.spiralRate = spiralRate;
            this.bulletCreationDistance = bulletCreationDistance;
            this.burstDuration = burstDuration;
            this.pauseDuration = pauseDuration;
            this.bulletSpeed = bulletSpeed;

            this.bulletAngleOffset = 360f / bulletDirections;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds;
            cycleTimer += (float)delta;
            elapsedTime += delta;

            // ⛔️ Stop movement while attacking
            enemy.CanMove = !inBurst;

            if (!inBurst)
            {
                if (cycleTimer >= pauseDuration)
                {
                    inBurst = true;
                    cycleTimer = 0;
                }
                return;
            }

            if (inBurst && cycleTimer >= burstDuration)
            {
                inBurst = false;
                cycleTimer = 0;
                spiralHeight = 0f;
                bulletIndex = 0;
                return;
            }

            if (elapsedTime >= spiralRate)
            {
                float angleDeg = bulletIndex * bulletAngleOffset;
                float angleRad = MathHelper.ToRadians(angleDeg);

                Vector2 offset = new Vector2(
                    (float)Math.Cos(angleRad),
                    (float)Math.Sin(angleRad)
                ) * bulletCreationDistance;

                Vector2 basePosition = new Vector2(
                    enemy.Position.X,
                    enemy.Position.Y + enemy.Size.Y / 2f + spiralHeight
                );

                EntityHandler.Add(new EnemyBullet(basePosition + offset, new StraightBullet(new Vector2(0, 1), bulletSpeed)));

                bulletIndex++;
                if (bulletIndex >= bulletDirections)
                {
                    bulletIndex = 0;
                    spiralHeight += spiralHeightStep;
                }

                elapsedTime = 0;
            }
        }
    }
}
