using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using CompileAndConquer.Content;


namespace CompileAndConquer.Entities.Enemy_Subtypes;

public class TypeB : Enemy
{
    private bool movingRight = true;
    private float leftBound = 10;
    private float rightBound = 1000;
    private int attackCooldown = 0;
    private int maxAttackCooldown = 150; // Fires every 2.5 seconds
    private List<EnemyBullet> pendingBullets = new List<EnemyBullet>();

    public TypeB(Texture2D texture, Vector2 position, IMovementStrategy strategy, IMovementStrategy bulletStrategy = null)
        : base(texture, position, 1.5f, 0.8f, strategy)
    {
        this.health = 120;
        this.BulletMovementStrategy = bulletStrategy;
    }

    public override void Update(GameTime gameTime)
    {
		base.Update(gameTime);

        if (attackCooldown > 0)
            attackCooldown--;

        if (attackCooldown == 0 && CanAttack())
        {
            Attack();
            attackCooldown = maxAttackCooldown;
        }

        foreach (var bullet in pendingBullets)
            EntityHandler.Add(bullet);

        pendingBullets.Clear();
    }

    public override void Attack()
    {
        pendingBullets.Add(new EnemyBullet(this.Position, BulletMovementStrategy));
    }
}
