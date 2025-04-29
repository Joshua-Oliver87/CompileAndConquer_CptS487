using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public class GatherBullet : Entity
    {
        private IMovementStrategy movementStrategy;
        
        private List<EnemyBullet> bulletsCollected = new List<EnemyBullet>();
        
        private float lifeTimer = 0f;
        private float maxLifetime = 3f;
        private bool controlled = false;

        public GatherBullet(Vector2 position, IMovementStrategy strategy)
        {
            this.texture = TextureHandler.Bullet;
            this.Position = position;
            this.movementStrategy = strategy;
            Scale = 4.0f;
            health = 1;
            Collidable = true;
        }

        public override void Update(GameTime gameTime)
        {
            lifeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (lifeTimer >= maxLifetime)
            {
                this.isAlive = false;
                return;
            }
            movementStrategy?.Move(this, gameTime);
            
            foreach (var bullet in bulletsCollected)
            {
                bullet.ControlPosition(this.Position);
            }
        }

        public override void OnCollide(Entity collision)
        {
            // Delete bullet on collision with player
            if (collision is Player)
                this.isAlive = false;
            
            if (collision is EnemyBullet bullet)
                bulletsCollected.Add(bullet);
        }
    }
}