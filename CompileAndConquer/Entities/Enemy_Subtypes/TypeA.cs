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

        public TypeA(Texture2D texture, Vector2 position, IMovementStrategy strategy, IAttackPattern attackPattern = null)
            : base(texture, position, 2.5f, 0.5f, strategy)
        {
            this.health = 80;
            this.attackPattern = attackPattern;
        }

        protected override void HandleAttack(GameTime gameTime)
        {
            if (!CanAttack()) return;
            attackPattern?.Update(this, gameTime);
        }

        public override void Attack()
        {

        }
    }
}
