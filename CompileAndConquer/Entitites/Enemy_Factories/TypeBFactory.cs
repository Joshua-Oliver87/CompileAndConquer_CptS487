using CompileAndConquer.Entities;
using CompileAndConquer.Entities.Enemy_Subtypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Entitites.Enemy_Factories;

public class TypeBFactory : IEnemyFactory
{
    private readonly Texture2D _texture;
    private readonly Vector2 _position;

    public TypeBFactory(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        _position = position;
    }

    public Enemy CreateEnemy()
    {
        return new TypeB(_texture, _position);
    }
}
