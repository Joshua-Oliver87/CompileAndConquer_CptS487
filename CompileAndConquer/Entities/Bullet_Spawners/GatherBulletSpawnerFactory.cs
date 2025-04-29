using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Entities.BulletSpawners
{
    public class GatherBulletSpawnerFactory
    {
        private readonly Vector2 position;
        private readonly float angleStep;
        private readonly float initialRadius;
        private readonly float radiusGrowthPerShot;
        private readonly float bulletSpeed;
        private readonly float bulletInterval;
        private readonly int maxShots;

        public GatherBulletSpawnerFactory(
            Vector2 position,
            float angleStep = 45f,
            float initialRadius = 10f,
            float radiusGrowthPerShot = 0f,
            float bulletSpeed = 200f,
            float bulletInterval = 0f,
            int maxShots = 5)
        {
            this.position = position;
            this.angleStep = angleStep;
            this.initialRadius = initialRadius;
            this.radiusGrowthPerShot = radiusGrowthPerShot;
            this.bulletSpeed = bulletSpeed;
            this.bulletInterval = bulletInterval;
            this.maxShots = maxShots;
        }

        public GatherBulletSpawner Create()
        {
            var spawner = new GatherBulletSpawner(position);
            
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