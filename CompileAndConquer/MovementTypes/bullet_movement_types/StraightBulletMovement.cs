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
            if (direction != Vector2.Zero)
                this.direction.Normalize();

            this.speed = speed;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            entity.Position += direction * speed * deltaTime;
            
            if (entity.Position.X < -entity.Size.X || entity.Position.X > CompileAndConquer.ScreenSize.X + entity.Size.X ||
                entity.Position.Y < -entity.Size.Y || entity.Position.Y > CompileAndConquer.ScreenSize.Y + entity.Size.Y)
            {
                entity.isAlive = false;
            }
        }
    }
}