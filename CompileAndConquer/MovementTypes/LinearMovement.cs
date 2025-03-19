using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public class LinearMovement : IMovementStrategy
    {
        private Vector2 direction;
        private float speed;

        public LinearMovement(Vector2 direction, float speed)
        {
            this.direction = direction;
            this.speed = speed;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            entity.Position += direction * speed;
        }
    }
}