using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace CompileAndConquer.Content;

public class FinalBoss : Enemy
{
    private int attackTimer = 0;
    private double lifetime = 15.0;

    private List<EnemyBullet> pendingBullets = new List<EnemyBullet>();

    public FinalBoss(Texture2D texture, Vector2 position) : base(texture, position, 1.0f, 2.0f, new ZigZagMovement(20, 2000, 1.5f))
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
