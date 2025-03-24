using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Content;


namespace CompileAndConquer.Entitites.Enemy_Factories
{
    public class FinalBossFactory : IEnemyFactory
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _position;

        public FinalBossFactory(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public Enemy CreateEnemy()
        {
            return new FinalBoss(_texture, _position);
        }
    }
}
