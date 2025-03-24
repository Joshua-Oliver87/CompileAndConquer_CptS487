using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entities;
using CompileAndConquer.Content;

namespace CompileAndConquer.Entitites.Enemy_Factories;

public class TypeAFactory : IEnemyFactory
{
    public Texture2D texture;
    public Vector2 position;
    public IMovementStrategy movement;

    public TypeAFactory(Texture2D newTexture, Vector2 newPosition, IMovementStrategy newMovement /*Behavior?*/)
    {
        texture = newTexture;
        position = newPosition;
        movement = newMovement;
    }

    public Enemy CreateEnemy()
    {
        return new TypeA(texture, position, movement);
    }
}

