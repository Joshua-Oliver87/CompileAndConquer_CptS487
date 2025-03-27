using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using CompileAndConquer.Content;


namespace CompileAndConquer.Entities.Enemy_Subtypes
{
    public class MiniBoss : Enemy
    {
        private int attackTimer = 0;
        private List<EnemyBullet> pendingBullets = new List<EnemyBullet>();

        public MiniBoss(Texture2D texture, Vector2 position, IMovementStrategy strategy, IMovementStrategy bulletMovement = null)
            : base(texture, position, 3.5f, 1.2f, strategy)
        {
            this.health = 300;
            this.BulletMovementStrategy = bulletMovement;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
       

            attackTimer++;
            if (attackTimer % 200 == 0 && CanAttack())
            {
                Attack();
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