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
    //// TO-DO: Add movement strategy here
    public TypeBFactory(Texture2D newTexture, Vector2 newPosition)
    {
        texture = newTexture;
        position = newPosition;
    }

    public Enemy CreateEnemy()
    {
        return new TypeB(texture, position);
    }
}
