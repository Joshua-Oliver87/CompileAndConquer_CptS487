using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content.MovementTypes.BulletMovementTypes
{
    public class StraightBullet : IMovementStrategy
    {
        private Vector2 direction;
        private float speed;

        public StraightBullet(Vector2 direction, float speed = 300f)
        {
            this.direction = direction;
            this.direction.Normalize();
            this.speed = speed;
        }
        public void Move(Entity entity, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            entity.Position += direction * speed * deltaTime;
        }
    }
}