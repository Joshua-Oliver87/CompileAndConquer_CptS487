using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using System;

namespace CompileAndConquer.Entities.Enemy_Subtypes
{
    public class TypeA : Enemy
    {
        private double stateTimer = 0;
        private bool isFiring = true;

        private readonly double firingDuration = 2.0;     // fire for 2 seconds
        private readonly double cooldownDuration = 1.5;   // pause for 1.5 seconds

        private IAttackPattern attackPattern;

        public TypeA(Texture2D texture, Vector2 position, IMovementStrategy strategy, IMovementStrategy bulletMovement = null, IAttackPattern attackPattern = null)
            : base(texture, position, 2.5f, 0.5f, strategy)
        {
            this.health = 80;
            this.BulletMovementStrategy = bulletMovement;
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
            // fallback if no attack pattern is provided
            if (BulletMovementStrategy != null)
            {
                Vector2 direction = Player.Instance.Position - this.Position;
                if (direction.Length() > 0)
                    direction.Normalize();

                EntityHandler.Add(new EnemyBullet(this.Position, BulletMovementStrategy));
            }
        }
    }
}
