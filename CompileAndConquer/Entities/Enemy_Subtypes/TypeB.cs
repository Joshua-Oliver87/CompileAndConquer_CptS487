using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities;

namespace CompileAndConquer.Entities.Enemy_Subtypes
{
    public class TypeB : Enemy
    {
        private readonly IAttackPattern _attackPattern;

        public TypeB(
            Texture2D texture,
            Vector2 startPosition,
            IMovementStrategy movementStrategy,
            IAttackPattern attackPattern       = null
        ) : base(texture, startPosition, 1.5f, 0.8f, movementStrategy)
        {
            this._attackPattern         = attackPattern;
            this.health                 = 120;
        }

        // Enemy.Update calls this under the hood:
        protected override void HandleAttack(GameTime gameTime)
        {
            // If there's an attack‐pattern, hand off to it
            if (_attackPattern != null)
            {
                _attackPattern.Update(this, gameTime);
            }
            else
            {
                // fallback one‐shot behavior
                if (CanAttack())
                    Attack();
            }
        }

        public override void Attack()
        {

        }
    }
}