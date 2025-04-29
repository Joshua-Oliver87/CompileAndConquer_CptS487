using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;

namespace CompileAndConquer.Entities.Enemy_Subtypes
{
    public class TypeB : Enemy
    {
        private readonly IAttackPattern _attackPattern;

        private double stateTimer = 0;
        private bool isFiring = true;

        private readonly double firingDuration = 2.0;
        private readonly double cooldownDuration = 1.5;

        public TypeB(
            Texture2D texture,
            Vector2 startPosition,
            IMovementStrategy movementStrategy,
            IAttackPattern attackPattern = null
        ) : base(texture, startPosition, 1.5f, 0.8f, movementStrategy)
        {
            this._attackPattern = attackPattern;
            this.health = 120;
        }

        protected override void HandleAttack(GameTime gameTime)
        {
            if (!CanAttack() || _attackPattern == null)
                return;

            stateTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (isFiring)
            {
                _attackPattern.Update(this, gameTime);

                if (stateTimer >= firingDuration)
                {
                    isFiring = false;
                    stateTimer = 0;
                }
            }
            else // in cooldown
            {
                if (stateTimer >= cooldownDuration)
                {
                    isFiring = true;
                    stateTimer = 0;
                    _attackPattern.Reset(); 
                }
            }
        }

        public override void Attack()
        {
            // Not used 
        }
    }
}