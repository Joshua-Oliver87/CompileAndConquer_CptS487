using Microsoft.Xna.Framework;
using CompileAndConquer.Content;
using CompileAndConquer.Entities.BulletSpawners;

namespace CompileAndConquer.Entities.BulletSpawners
{
    public class ThreeWaySpiralSpawnerFactory
    {
        private readonly Vector2 position;
        private readonly float angleStep;
        private readonly float initialRadius;
        private readonly float radiusGrowth;
        private readonly float bulletSpeed;
        private readonly float bulletInterval;
        private readonly int   maxShots;

        public ThreeWaySpiralSpawnerFactory(
            Vector2 position,
            float angleStep       = 30f,
            float initialRadius   = 20f,
            float radiusGrowth    = 1f,
            float bulletSpeed     = 120f,
            float bulletInterval  = 0.04f,
            int   maxShots        = 60
        ) {
            this.position       = position;
            this.angleStep      = angleStep;
            this.initialRadius  = initialRadius;
            this.radiusGrowth   = radiusGrowth;
            this.bulletSpeed    = bulletSpeed;
            this.bulletInterval = bulletInterval;
            this.maxShots       = maxShots;
        }

        public ThreeWaySpiralSpawner Create()
        {
            var s = new ThreeWaySpiralSpawner(position)
            {
                AngleStep           = angleStep,
                SpiralRadius        = initialRadius,
                RadiusGrowthPerShot = radiusGrowth,
                BulletSpeed         = bulletSpeed,
                BulletInterval      = bulletInterval,
                MaxShots            = maxShots
            };
            return s;
        }
    }
}