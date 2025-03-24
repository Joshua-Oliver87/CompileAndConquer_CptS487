using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Content;

namespace CompileAndConquer.Entitites.Enemy_Factories
{
    public class FinalBossFactory : IEnemyFactory
    {
        public Texture2D texture;
        public Vector2 position;

        public FinalBossFactory(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;
        }

        public Enemy CreateEnemy()
        {
            return new FinalBoss(texture, position);
        }
    }
}
