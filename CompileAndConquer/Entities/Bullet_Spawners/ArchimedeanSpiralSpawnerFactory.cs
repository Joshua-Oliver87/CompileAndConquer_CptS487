// Entities/BulletSpawners/ArchimedeanSpiralSpawnerFactory.cs
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Entities.BulletSpawners
{
    public class ArchimedeanSpiralSpawnerFactory
    {
        private readonly Vector2 center;
        private readonly float a, b, angSpeed, interval, bulletSpeed;
        private readonly int   maxShots;

        public ArchimedeanSpiralSpawnerFactory(
            Vector2 center,
            float a          = 20f,
            float b          =  5f,
            float angSpeed   =  3f,
            float interval   =  0.05f,
            int   maxShots   = 60,
            float bulletSpeed=120f
        ) {
            this.center      = center;
            this.a           = a;
            this.b           = b;
            this.angSpeed    = angSpeed;
            this.interval    = interval;
            this.maxShots    = maxShots;
            this.bulletSpeed = bulletSpeed;
        }

        public ArchimedeanSpiralSpawner Create()
        {
            return new ArchimedeanSpiralSpawner(
                center, a, b, angSpeed, interval, maxShots, bulletSpeed
            );
        }
    }
}