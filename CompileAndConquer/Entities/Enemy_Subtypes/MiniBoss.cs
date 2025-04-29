using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using System;

namespace CompileAndConquer.Entities.Enemy_Subtypes
{
    public class MiniBoss : Enemy
    {
        private double stateTimer = 0;
        private bool isFiring = true;

        private readonly double firingDuration = 3.0;  // Fire for 3 seconds (longer than TypeA for boss feel)
        private readonly double cooldownDuration = 2.0; // Pause for 2 seconds (longer cooldown)

        private IAttackPattern attackPattern;

        public MiniBoss(Texture2D texture, Vector2 position, IMovementStrategy strategy, IAttackPattern attackPattern = null)
            : base(texture, position, 3.5f, 1.2f, strategy)
        {
            this.health = 300;
            this.attackPattern = attackPattern;
        }

        protected override void HandleAttack(GameTime gameTime)
        {
            if (!CanAttack())
                return;

            stateTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (isFiring)
            {
                attackPattern?.Update(this, gameTime);

                if (stateTimer >= firingDuration)
                {
                    isFiring = false;
                    stateTimer = 0;
                }
            }
            else // cooldown
            {
                if (stateTimer >= cooldownDuration)
                {
                    isFiring = true;
                    stateTimer = 0;
                }
            }
        }

        public override void Attack()
        {

        }
    }
}