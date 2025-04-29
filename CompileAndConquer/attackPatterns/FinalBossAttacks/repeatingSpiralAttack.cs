using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.Entities.BulletSpawners;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class RepeatingSpiralAttack : IAttackPattern
    {
        private float burstDuration   = 2.0f;
        private float pauseDuration   = 4.8f;
        private float cycleTimer      = 0f;
        private bool  inBurst         = false;
        private float timer           = 0f;
    
        // track our actual spawners
        private SpiralBulletSpawner topSpawner;
        private SpiralBulletSpawner botSpawner;
    
        public bool IsAttackInProgress
            => inBurst 
               || (topSpawner?.isAlive == true) 
               || (botSpawner?.isAlive == true);
    
        public void Reset()
        {
            timer      = 0f;
            cycleTimer = 0f;
            inBurst    = true;
            topSpawner = null;
            botSpawner = null;
        }
    
        public void Update(Enemy enemy, GameTime gameTime)
        {
            cycleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            enemy.CanMove = !IsAttackInProgress;
    
            if (!inBurst)
            {
                // once done, wait out the pauseDuration before starting again
                if (cycleTimer >= pauseDuration)
                {
                    cycleTimer = 0f;
                    inBurst    = true;
                    timer      = 0f;
                    topSpawner = null;
                    botSpawner = null;
                }
                return;
            }
    
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
    
            // spawn two spirals exactly once
            if (topSpawner == null && timer <= burstDuration)
            {
                Vector2 basePos = enemy.Position + new Vector2(0, enemy.Size.Y/2f);
    
                topSpawner = (SpiralBulletSpawner)new SpiralBulletSpawnerFactory(basePos).Create();
                topSpawner.SpiralHeight = 0f;
                topSpawner.HeightStep   = Math.Abs(topSpawner.HeightStep);
                topSpawner.AngleStep    = Math.Abs(topSpawner.AngleStep);
                EntityHandler.Add(topSpawner);
    
                botSpawner = (SpiralBulletSpawner)new SpiralBulletSpawnerFactory(basePos).Create();
                botSpawner.SpiralHeight = botSpawner.MaxShots * Math.Abs(botSpawner.HeightStep) - 50f;
                botSpawner.HeightStep   = -Math.Abs(botSpawner.HeightStep);
                botSpawner.AngleStep    = -Math.Abs(botSpawner.AngleStep);
                EntityHandler.Add(botSpawner);
            }
    
            // once our burst window is over *and* both spawners have died,
            // we can flip out of inBurst and let the pause begin:
            if (timer >= burstDuration
                && (topSpawner?.isAlive != true)
                && (botSpawner?.isAlive != true))
            {
                inBurst = false;
            }
        }
    }

}
