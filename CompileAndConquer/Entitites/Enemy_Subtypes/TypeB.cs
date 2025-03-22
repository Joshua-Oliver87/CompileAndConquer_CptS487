using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CompileAndConquer.Content;

public class TypeB : Enemy
{
    private bool movingRight = true;
    private float leftBound = 10;
    private float rightBound = 1000;
    private int attackCooldown = 0;
    private int maxAttackCooldown = 150; // Fires every 2.5 seconds
    private List<EnemyBullet> pendingBullets = new List<EnemyBullet>();

    public TypeB(Texture2D texture, Vector2 position) : base(texture, position, 2.5f, 0.75f, new ZigZagMovement(10, 1000, 2.5f))
    {
        this.health = 120;
    }

    public override void Update(GameTime gameTime)
    {
		base.Update(gameTime);

        if (attackCooldown > 0)
            attackCooldown--;

        if (attackCooldown == 0)
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
        Vector2 direction = Player.Instance.Position - this.Position;
        if (direction.Length() > 0)
            direction.Normalize();

        pendingBullets.Add(new EnemyBullet(this.Position, direction * 5.0f));
    }
}
