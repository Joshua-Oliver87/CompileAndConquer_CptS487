using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entities;
using CompileAndConquer.Content;

namespace CompileAndConquer.Entitites.Enemy_Factories
{
    public class MiniBossFactory : IEnemyFactory
    {
        private readonly Texture2D texture;
        private readonly Vector2 position;
        
        public IMovementStrategy movement;
        public IMovementStrategy bulletMovement;

        public MiniBossFactory(Texture2D newTexture, Vector2 newPosition, IMovementStrategy newMovement, IMovementStrategy bulletMovement)
        {
            texture = newTexture;
            position = newPosition;
            movement = newMovement;
            this.bulletMovement = bulletMovement;
        }

        public Enemy CreateEnemy()
        {
            return new MiniBoss(texture, position, movement, bulletMovement);
        }
    }
}
