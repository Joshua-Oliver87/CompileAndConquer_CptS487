using Microsoft.Xna.Framework;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using System;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class ThreeWaySpiralAttack : IAttackPattern
    {
        private readonly float burstDuration;
        private float burstTimer;
        private bool isInBurst;

        private readonly Func<Vector2, ArchimedeanSpiralSpawner> spawnerFactory;
        private bool hasSpawned;

        public bool IsAttackInProgress => isInBurst;

        public ThreeWaySpiralAttack(
            float burstDuration = 5f,
            Func<Vector2, ArchimedeanSpiralSpawner>? spawnerFactory = null
        )
        {
            this.burstDuration = burstDuration;
            this.spawnerFactory = spawnerFactory ?? DefaultSpawnerFactory;
            Reset();
        }

        public void Reset()
        {
            burstTimer = 0f;
            isInBurst = false;
            hasSpawned = false;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isInBurst)
            {
                isInBurst = true;
                burstTimer = 0f;
                hasSpawned = false;
            }

            if (isInBurst && !hasSpawned)
            {
                hasSpawned = true;
                var spawnPos = enemy.Position + new Vector2(0, enemy.Size.Y / 2f);
                var spawner = spawnerFactory(spawnPos);
                EntityHandler.Add(spawner);
            }

            burstTimer += dt;

            if (burstTimer >= burstDuration)
            {
                Reset(); // end burst cycle, ready to start again if simple enemy
            }
        }

        private ArchimedeanSpiralSpawner DefaultSpawnerFactory(Vector2 spawnPos)
        {
            return new ArchimedeanSpiralSpawnerFactory(
                center: spawnPos,
                a: 20f,
                b: 5f,
                angSpeed: 8f,
                interval: 0.01f,
                maxShots: 60,
                bulletSpeed: 220f
            ).Create();
        }
    }
}
