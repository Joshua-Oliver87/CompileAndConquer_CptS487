using CompileAndConquer.Content;
using CompileAndConquer.Entities;
using CompileAndConquer.Entities.Enemy_Subtypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Entitites.Enemy_Factories;

public class TypeBFactory : IEnemyFactory
{
    public Texture2D texture;
    public Vector2 position;
    public IMovementStrategy movement;
    private IMovementStrategy bulletMovement;
    public TypeBFactory(Texture2D newTexture, Vector2 newPosition, IMovementStrategy newMovement, IMovementStrategy bulletMovement = null)
    {
        texture = newTexture;
        position = newPosition;
        movement = newMovement;
        this.bulletMovement = bulletMovement; 
    }

    public Enemy CreateEnemy()
    {
        return new TypeB(texture, position, movement, bulletMovement);
    }
}
