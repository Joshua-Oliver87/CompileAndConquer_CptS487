using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public class EnemyBullet : Entity
    {
        private IMovementStrategy movementStrategy;

        public EnemyBullet(Vector2 position, IMovementStrategy strategy)
        {
            this.texture = TextureHandler.Bullet; // Make sure Bullet texture exists
            this.Position = position;
            this.movementStrategy = strategy;
            Scale = 2.0f;
            health = 1;
            Collidable = true;
        }

        public override void Update(GameTime gameTime)
        {
            movementStrategy?.Move(this, gameTime);

            // Despawn bullet if it's off screen
            if (Position.Y < -Size.Y || Position.Y > CompileAndConquer.ScreenSize.Y ||
                Position.X < -Size.X || Position.X > CompileAndConquer.ScreenSize.X)
            {
                isAlive = false;
                EntityHandler.RemoveEntity(this);
            }
        }

        public override void OnCollide(Entity collision)
        {
            // Delete bullet on collision with player
            if (collision is Player)
                this.isAlive = false;
        }
    }
}