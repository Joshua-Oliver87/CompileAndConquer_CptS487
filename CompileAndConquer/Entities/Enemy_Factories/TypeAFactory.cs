using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entities;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;

namespace CompileAndConquer.Entitites.Enemy_Factories;

public class TypeAFactory : IEnemyFactory
{
    public Texture2D texture;
    public Vector2 position;
    public readonly Func<IMovementStrategy> _movementFactory;
    public IMovementStrategy bulletMovement;
    public IAttackPattern attackPattern;

    public TypeAFactory(Texture2D newTexture, Vector2 newPosition, Func<IMovementStrategy> movementFactory, IMovementStrategy bulletMovement = null, IAttackPattern attackPattern = null)
    {
        texture = newTexture;
        position = newPosition;
        _movementFactory = movementFactory;
        this.bulletMovement = bulletMovement;
        this.attackPattern = attackPattern;
    }

    public Enemy CreateEnemy()
    {
		var movement = _movementFactory();
        return new TypeA(texture, position, movement, bulletMovement, attackPattern);
    }
}

