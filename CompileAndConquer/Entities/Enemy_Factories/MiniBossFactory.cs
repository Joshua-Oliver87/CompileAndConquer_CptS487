// Entities/Enemy_Factories/MiniBossFactory.cs
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Content;

namespace CompileAndConquer.Entities.Enemy_Factories
{
    public class MiniBossFactory : IEnemyFactory
    {
        private readonly Texture2D texture;
        private readonly Vector2 position;
        private readonly Func<IMovementStrategy> _movementFactory;
        private readonly Func<IAttackPattern>  _attackFactory;

        public MiniBossFactory(
            Texture2D texture,
            Vector2 position,
            Func<IMovementStrategy> movementFactory,
            Func<IAttackPattern>   attackFactory         = null)
        {
            this.texture                 = texture;
            this.position                = position;
            this._movementFactory        = movementFactory;
            this._attackFactory          = attackFactory;
        }

        public Enemy CreateEnemy()
        {
            var move    = _movementFactory();
            var attack  = _attackFactory?.Invoke();
            return new MiniBoss(texture, position, move, attack);
        }
    }
}