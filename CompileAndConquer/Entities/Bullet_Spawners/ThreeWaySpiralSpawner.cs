// Entities/BulletSpawners/ThreeWaySpiralSpawner.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

namespace CompileAndConquer.Entities.BulletSpawners
{
    public class ThreeWaySpiralSpawner : Entity
    {
        // configurable properties
        public float AngleStep { get; set; }
        public float SpiralRadius { get; set; }
        public float RadiusGrowthPerShot { get; set; }
        public float BulletSpeed { get; set; }
        public float BulletInterval { get; set; }
        public int   MaxShots       { get; set; }

        // internal state
        private float timeSinceLast;
        private int   shotsFired;
        private float angleOffset;
        private Vector2 origin;

        public ThreeWaySpiralSpawner(Vector2 position)
        {
            origin        = position;
            Position      = position;
            texture       = TextureHandler.Pixel; // 1×1 transparent
            Scale         = 0f;
            health        = 1;

            // your defaults (you can override via factory)
            AngleStep         = 30f;
            SpiralRadius      = 20f;
            RadiusGrowthPerShot = 1f;
            BulletSpeed       = 120f;
            BulletInterval    = 0.04f;
            MaxShots          = 60;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLast += dt;

            if (shotsFired >= MaxShots)
            {
                isAlive = false;
                return;
            }

            if (timeSinceLast < BulletInterval)
                return;

            // compute current ring radius & spawn point
            float rad   = MathHelper.ToRadians(angleOffset);
            Vector2 ringOffset = new Vector2((float)System.Math.Cos(rad), (float)System.Math.Sin(rad)) * SpiralRadius;
            Vector2 spawnPos   = origin + ringOffset;

            // --- 1) bullet toward player ---
            Vector2 toPlayer = Player.Instance.Position - spawnPos;
            if (toPlayer != Vector2.Zero) toPlayer.Normalize();
            EntityHandler.Add(new EnemyBullet(spawnPos, new StraightBullet(toPlayer, BulletSpeed)));

            // --- 2) two side streams at ±120° ---
            for (int i = 1; i < 3; i++)
            {
                float ang = angleOffset + 120f * i;
                float r2  = MathHelper.ToRadians(ang);
                Vector2 dir = new Vector2((float)System.Math.Cos(r2), (float)System.Math.Sin(r2));
                EntityHandler.Add(new EnemyBullet(spawnPos, new StraightBullet(dir, BulletSpeed)));
            }

            // advance spiral
            angleOffset       = (angleOffset + AngleStep) % 360f;
            SpiralRadius     += RadiusGrowthPerShot;
            shotsFired       += 1;
            timeSinceLast     = 0f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // invisible
        }
    }
}
