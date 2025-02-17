using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CompileAndConquer.Content;

public class TypeA : Enemy
{
    private int attackCooldown = 0;
    private int maxAttackCooldown = 120; // Fires every 2 seconds
    private List<EnemyBullet> pendingBullets = new List<EnemyBullet>();

    public TypeA(Texture2D texture, Vector2 position) : base(texture, position, 2.5f, 0.5f)
    {
        this.health = 80;
    }

    public override void Update()
    {
        Vector2 direction = Player.Instance.Position - this.Position;
        if (direction.Length() > 0)
        {
            direction.Normalize();
        }

        this.Position += direction * Speed;
        
        // Update collider will change later
        CollisionRectangle.X = (int)Position.X - (texture.Width / 2 * (int)ScaleFactor);
        CollisionRectangle.Y = (int)Position.Y - (texture.Height / 2 * (int)ScaleFactor);

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
