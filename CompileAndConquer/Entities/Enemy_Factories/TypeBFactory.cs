using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities;
using CompileAndConquer.Entities.Enemy_Subtypes;

namespace CompileAndConquer.Entities.Enemy_Factories
{
    public class TypeBFactory : IEnemyFactory
    {
        private readonly Texture2D                  _texture;
        private readonly Vector2                    _startPosition;
        private readonly Func<IMovementStrategy>    _movementFactory;
        private readonly Func<IAttackPattern>       _attackFactory;

        public TypeBFactory(
            Texture2D                texture,
            Vector2                  startPosition,
            Func<IMovementStrategy>  movementFactory,
            Func<IAttackPattern>     attackFactory    = null
        ) {
            _texture          = texture;
            _startPosition    = startPosition;
            _movementFactory  = movementFactory;
            _attackFactory    = attackFactory;
        }

        public Enemy CreateEnemy()
        {
            var movement = _movementFactory();
            var attack   = _attackFactory is null ? null : _attackFactory();
            return new TypeB(
                _texture,
                _startPosition,
                movement,
                attack
            );
        }
    }
}
