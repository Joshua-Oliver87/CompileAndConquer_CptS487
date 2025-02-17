using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace CompileAndConquer.Content;

public class MiniBoss : Enemy
{
    private int attackTimer = 0;
	private double lifetime = 15.0;
    private List<EnemyBullet> pendingBullets = new List<EnemyBullet>();

    public MiniBoss(Texture2D texture, Vector2 position) : base(texture, position, 1.5f, 1.75f)
    {
        this.health = 300;
    }

    public override void Update()
    {
		lifetime -= 1.0 / 60.0;
		if (lifetime <= 0)
		{
            EntityHandler.ScheduleClearEnemies();
            EntityHandler.RemoveEntity(this);
		}
        Vector2 direction = Player.Instance.Position - this.Position;
        if (direction.Length() > 0)
            direction.Normalize();

        this.Position += direction * Speed;

        attackTimer++;
        if (attackTimer % 200 == 0)
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

        pendingBullets.Add(new EnemyBullet(this.Position, direction * 5.5f));
    }
}
