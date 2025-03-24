using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entities;

namespace CompileAndConquer.Entitites.Enemy_Factories
{
    public class MiniBossFactory : IEnemyFactory
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _position;

        public MiniBossFactory(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public Enemy CreateEnemy()
        {
            return new MiniBoss(_texture, _position);
        }
    }
}
