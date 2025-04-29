using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using System;

namespace CompileAndConquer.Entities.BulletSpawners
{
    public class GatherBulletSpawner : Entity
    {
        private float angle = 0f;
        private float timeSinceLastBullet = 0f;
        private int shotsFired = 0;

        private Vector2 origin;
        
        public float AngleStep { get; set; }
        public float SpiralRadius { get; set; }
        public float RadiusGrowthPerShot { get; set; }
        public float BulletSpeed { get; set; }
        public float BulletInterval { get; set; }
        public int MaxShots { get; set; }

        public float SpiralHeight = 0f;
        public float HeightStep = 2f;
		private Color color = Color.Blue;

        public GatherBulletSpawner(Vector2 position)
        {
            this.origin = position;
            this.Position = position;
            this.texture = TextureHandler.Pixel;
            this.Scale = 0f;
            this.health = 1;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastBullet += dt;

            if (shotsFired >= MaxShots)
            {
                isAlive = false;
                return;
            }

            if (timeSinceLastBullet >= BulletInterval)
            {
                float radians = MathHelper.ToRadians(angle);
                Vector2 spiralOffset = new Vector2(
                    (float)Math.Cos(radians),
                    (float)Math.Sin(radians)
                ) * SpiralRadius;

                SpiralRadius += RadiusGrowthPerShot;

                Vector2 spawnPosition = origin + spiralOffset + new Vector2(0, SpiralHeight);

                float r2  = MathHelper.ToRadians(angle);
                Vector2 dir = new Vector2((float)System.Math.Cos(r2), (float)System.Math.Sin(r2));

                var bullet = new GatherBullet(spawnPosition, new StraightBullet(dir, BulletSpeed), color);
                EntityHandler.Add(bullet);

                angle -= AngleStep;
                SpiralHeight += HeightStep / (360f / AngleStep);
                shotsFired++;
                timeSinceLastBullet = 0f;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) { }
    }
}
