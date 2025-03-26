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
        
        public IMovementStrategy movement;

        public FinalBossFactory(Texture2D newTexture, Vector2 newPosition, IMovementStrategy newMovement)
        {
            texture = newTexture;
            position = newPosition;
            movement = newMovement;
        }

        public Enemy CreateEnemy()
        {
            return new FinalBoss(texture, position, movement);
        }
    }
}
