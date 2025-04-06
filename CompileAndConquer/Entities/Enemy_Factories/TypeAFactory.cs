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
    public IMovementStrategy movement;
    public IMovementStrategy bulletMovement;
    public IAttackPattern attackPattern;

    public TypeAFactory(Texture2D newTexture, Vector2 newPosition, IMovementStrategy newMovement, IMovementStrategy bulletMovement = null, IAttackPattern attackPattern = null)
    {
        texture = newTexture;
        position = newPosition;
        movement = newMovement;
        this.bulletMovement = bulletMovement;
        this.attackPattern = attackPattern;
    }

    public Enemy CreateEnemy()
    {
        return new TypeA(texture, position, movement, bulletMovement, attackPattern);
    }
}

