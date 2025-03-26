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

        public MiniBossFactory(Texture2D newTexture, Vector2 newPosition, IMovementStrategy newMovement)
        {
            texture = newTexture;
            position = newPosition;
            movement = newMovement;
        }

        public Enemy CreateEnemy()
        {
            return new MiniBoss(texture, position, movement);
        }
    }
}
