using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Entities.BulletSpawners
{
    public class SpiralBulletSpawnerFactory
    {
        private readonly Vector2 position;
        private readonly float angleStep;
        private readonly float initialRadius;
        private readonly float radiusGrowthPerShot;
        private readonly float bulletSpeed;
        private readonly float bulletInterval;
        private readonly int maxShots;

        public SpiralBulletSpawnerFactory(
            Vector2 position,
            float angleStep = 42.5f,
            float initialRadius = 20f,
            float radiusGrowthPerShot = 1.2f,
            float bulletSpeed = 60f,
            float bulletInterval = 0.02f,
            int maxShots = 45)
        {
            this.position = position;
            this.angleStep = angleStep;
            this.initialRadius = initialRadius;
            this.radiusGrowthPerShot = radiusGrowthPerShot;
            this.bulletSpeed = bulletSpeed;
            this.bulletInterval = bulletInterval;
            this.maxShots = maxShots;
        }

        public SpiralBulletSpawner Create()
        {
            var spawner = new SpiralBulletSpawner(position);
            
            spawner.AngleStep = angleStep;
            spawner.SpiralRadius = initialRadius;
            spawner.RadiusGrowthPerShot = radiusGrowthPerShot;
            spawner.BulletSpeed = bulletSpeed;
            spawner.BulletInterval = bulletInterval;
            spawner.MaxShots = maxShots;

            return spawner;
        }
    }
}