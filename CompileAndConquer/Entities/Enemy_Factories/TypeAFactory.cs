using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entities;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;

namespace CompileAndConquer.Entities.Enemy_Factories;

public class TypeAFactory : IEnemyFactory
{
    public Texture2D texture;
    public Vector2 position;
    public readonly Func<IMovementStrategy> _movementFactory;
    public readonly Func<IAttackPattern>    _attackFactory;

    public TypeAFactory(Texture2D newTexture, Vector2 newPosition, Func<IMovementStrategy> movementFactory, Func<IAttackPattern> attackFactory = null)
    {
        texture = newTexture;
        position = newPosition;
        _movementFactory = movementFactory;
        _attackFactory = attackFactory;
    }

    public Enemy CreateEnemy()
    {
        var move    = _movementFactory();
        var attack = _attackFactory?.Invoke();
        return new TypeA(texture, position, move, attack);
    }
}

