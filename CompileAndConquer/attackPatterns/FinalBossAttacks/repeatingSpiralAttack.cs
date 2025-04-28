using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.Entities.BulletSpawners;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class RepeatingSpiralAttack : IAttackPattern
    {
        private float burstDuration   = 1.0f;
        private float pauseDuration   = 3.8f;
        private float cycleTimer      = 5f;      // ← never reset in Reset()
        private bool  inBurst         = false;
        private float timer           = 0f;
        private bool  hasFired;                  // ← once true, you bail forever
        private bool  spawnersCreated = false;
    
        public bool IsAttackInProgress => inBurst;
    
        public void Reset()
        {
            timer           = 0f;
            hasFired        = false;
            cycleTimer      = 0f;
            inBurst         = false;
            spawnersCreated = false;
        }
    
        public void Update(Enemy enemy, GameTime gameTime)
        {
            if (hasFired) return;  // ← once this flips, you never fire again
    
            cycleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            enemy.CanMove = !inBurst;
    
            if (!inBurst)
            {
                if (cycleTimer >= pauseDuration)
                {
                    inBurst    = true;
                    cycleTimer = 0;
                }
                return;
            }
    
            // spawn for burstDuration seconds
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= burstDuration)
            {
                inBurst   = false;
                timer     = 0f;
                hasFired  = false;   // ← if you really want to repeat, don’t set hasFired = true here
                return;
            }
    
            if (!spawnersCreated)
            {
               spawnersCreated = true;
               Vector2 basePos = new Vector2(
                        enemy.Position.X,
                        enemy.Position.Y + enemy.Size.Y / 2f
                    );
    
                // Offset left and right to form dual spirals
                float offsetX = 20f;

                var leftFactory = new SpiralBulletSpawnerFactory(basePos + new Vector2(-offsetX, 0));
                EntityHandler.Add(leftFactory.Create());

                var rightFactory = new SpiralBulletSpawnerFactory(basePos + new Vector2(offsetX, 0));
                EntityHandler.Add(rightFactory.Create());
            }
        }
    }
}
