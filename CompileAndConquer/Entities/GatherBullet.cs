using System.Collections.Generic;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public class GatherBullet : EnemyBullet
    {
        private List<EnemyBullet> bulletsCollected = new List<EnemyBullet>();
        
        public GatherBullet(Vector2 position, IMovementStrategy strategy, Color tint) : base(position, strategy, tint)
        {
            this.texture = TextureHandler.GatherBullet;
            this.Position = position;
            this.movementStrategy = strategy;
            Scale = 3.0f;
            health = 1;
            Collidable = true;
            this.color = tint;
            this.maxLifetime = 100f;
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

            if (bulletsCollected.Count != 0)
            {
                int offset = -25;
                foreach (var bullet in bulletsCollected)
                {
                    Vector2 newPos = new Vector2(this.Position.X + offset, this.Position.Y + 30);
                    bullet.ControlPosition(newPos);
                    offset += 5;
                    if (offset > 25)
                        offset = -25;
                }
            }

            if (!this.isAlive)
            {
                // foreach (var bullet in bulletsCollected)
                // {
                //     bullet.lifeTimer = 0
                //     bullet = StraightBullet(dir, BulletSpeed);
                // }
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