using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public class EnemyBullet : Entity
    {
        private IMovementStrategy movementStrategy;
        
        private float lifeTimer = 0f;
        private float maxLifetime = 3f;
        private bool controlled = false;

        

        public EnemyBullet(Vector2 position, IMovementStrategy strategy, Color tint)
        {
            this.texture = TextureHandler.Bullet;
            this.Position = position;
            this.movementStrategy = strategy;
            Scale = 2.0f;
            health = 1;
            Collidable = true;
			this.color = tint;
        }

        public override void Update(GameTime gameTime)
        {
            movementStrategy?.Move(this, gameTime);
            
        }
        
        public void ControlPosition(Vector2 pos)
        {
            if (controlled)
                this.Position = pos;
        }
        public override void OnCollide(Entity collision)
        {
            // Delete bullet on collision with player
            if (collision is Player)
                this.isAlive = false;

            if (collision is GatherBullet)
            {
                controlled = true;
                this.movementStrategy = null;
            }
        }
    }
}