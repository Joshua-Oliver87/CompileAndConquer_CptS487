using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using CompileAndConquer.Content;
using System;
using CompileAndConquer.Content;


namespace CompileAndConquer.Entities.Enemy_Subtypes
{
    public class FinalBoss : Enemy
    {
        private int attackTimer = 0;

        private List<EnemyBullet> pendingBullets = new List<EnemyBullet>();

        public FinalBoss(Texture2D texture, Vector2 position, IMovementStrategy strategy, IMovementStrategy bulletMovement = null)
            : base(texture, position, 4.5f, 1.5f, strategy)
        {
            this.health = 1000;
            this.BulletMovementStrategy = bulletMovement;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            attackTimer++;
            if (attackTimer % 150 == 0 && CanAttack())
            {
                Attack();
            }
            
            if (health <= 0)
            {
                EntityHandler.RemoveEntity(this);
            }

            foreach (var bullet in pendingBullets)
                EntityHandler.Add(bullet);
            pendingBullets.Clear();
        }

        public override void Attack()
        {
            Vector2 direction = Player.Instance.Position - this.Position;
            if (direction.Length() > 0)
                direction.Normalize();

            pendingBullets.Add(new EnemyBullet(this.Position, BulletMovementStrategy));
        }
    }    
}

