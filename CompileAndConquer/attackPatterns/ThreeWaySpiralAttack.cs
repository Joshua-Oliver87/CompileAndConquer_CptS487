using Microsoft.Xna.Framework;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class ThreeWaySpiralAttack : IAttackPattern
    {
        private float burstDur = 3f, pauseDur = 2f, timer;
        private bool  inBurst, spawned;

        public void Update(Enemy boss, GameTime gt)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            timer += dt;
            boss.CanMove = !inBurst;

            if (!inBurst)
            {
                if (timer >= pauseDur)
                {
                    inBurst  = true;
                    timer    = 0;
                    spawned  = false;
                }
                return;
            }
            if (inBurst && timer >= burstDur)
            {
                inBurst = false;
                timer   = 0;
                return;
            }
            if (inBurst && !spawned)
            {
                var basePos = boss.Position + new Vector2(0, boss.Size.Y/2f);
                var factory = new ArchimedeanSpiralSpawnerFactory(
                    basePos,
                    a:20f, b:5f, angSpeed:3f,
                    interval:0.04f, maxShots:60, bulletSpeed:120f
                );
                EntityHandler.Add(factory.Create());
                spawned = true;
            }
        }
    }
}