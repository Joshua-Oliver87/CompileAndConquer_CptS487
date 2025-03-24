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
        private double lifetime = 15.0;

        private List<EnemyBullet> pendingBullets = new List<EnemyBullet>();

        public FinalBoss(Texture2D texture, Vector2 position) : base(texture, position, 1.0f, 2.0f, new ZigZagMovement(
            leftBound: 100,
            rightBound: 800,
            topBound: 100,
            bottomBound: CompileAndConquer.ScreenSize.Y * 0.25f))
        {
            this.health = 1000;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            attackTimer++;
            if (attackTimer % 150 == 0)
            {
                Attack();
            }

            lifetime -= 1.0 / 60.0;
            if (lifetime <= 0)
            {
                EntityHandler.ScheduleClearEnemies();
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

            pendingBullets.Add(new EnemyBullet(this.Position, direction * 6.0f));
        }
    }    
}

