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
        public IMovementStrategy bulletMovement;

        public FinalBossFactory(Texture2D newTexture, Vector2 newPosition, IMovementStrategy newMovement, IMovementStrategy bulletMovement)
        {
            texture = newTexture;
            position = newPosition;
            movement = newMovement;
            this.bulletMovement = bulletMovement;
        }

        public Enemy CreateEnemy()
        {
            return new FinalBoss(texture, position, movement, bulletMovement);
        }
    }
}
