using CompileAndConquer.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Entitites.Enemy_Factories

public class TypeAFactory : IEnemyFactory
{
    private readonly Texture2D _texture;
    private readonly Vector2 _position;

    public TypeAFactory(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        _position = position;
    }

    public Enemy CreateEnemy()
    {
        return new TypeA(_texture, _position);
    }
}

