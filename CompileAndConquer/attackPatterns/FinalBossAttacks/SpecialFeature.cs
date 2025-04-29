using System;
using Microsoft.Xna.Framework;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class SpecialFeature : IAttackPattern
    {
        private readonly float burstDur = 7f;
        private float          timer;
		private bool 		   inBurst;
        private bool           spawned;

        public void Reset()
        {
            timer   = 0f;
			inBurst = true;
            spawned = false;
        }

        // Pattern remains in progress until the initial spiral spawner is created.
        public bool IsAttackInProgress => inBurst;

        public void Update(Enemy boss, GameTime gt)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            timer += dt;

            // Fire initial spiral once, then let the spawner handle bullet emission over its lifetime.
            if (inBurst && !spawned && timer >= 0f)
            {
                spawned = true;
                var basePos = boss.Position + new Vector2(0, boss.Size.Y / 2f);
                var spawner = new ArchimedeanSpiralSpawnerFactory(
                    basePos, a:20f, b:5f, angSpeed:8f,
                    interval:0.0f, maxShots:120, bulletSpeed:80f
                ).Create();

                EntityHandler.Add(spawner);
            }
            if (inBurst && timer >= burstDur)
            { 
                inBurst = false;
                var basePos = boss.Position + new Vector2(0, boss.Size.Y / 2f);

                var botSpawner = (GatherBulletSpawner)new GatherBulletSpawnerFactory(basePos).Create();
                botSpawner.SpiralHeight = botSpawner.MaxShots * Math.Abs(botSpawner.HeightStep) - 50f;
                botSpawner.HeightStep   = -Math.Abs(botSpawner.HeightStep);
                botSpawner.AngleStep    = -Math.Abs(botSpawner.AngleStep);
                EntityHandler.Add(botSpawner);
            }
        }
    }
}