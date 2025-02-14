using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

// Stronger enemy with melee + ranged attacks
public class MiniBoss : Enemy
{
    private int attackTimer = 0;
    public MiniBoss(Texture2D texture, Vector2 position) : base(texture, position, 1.5f, 1.75f)
    {
        this.health = 300; // default
    }

    // Attacks every once in a while
    public override void Update()
    {
        // moves towards player slowly while attacking
        Vector2 direction = Player.Instance.Position - this.Position;
        if (direction.Length() > 0)
            direction.Normalize();

        this.Position += direction * Speed;

        attackTimer++;
        if (attackTimer % 200 == 0)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        if (attackTimer % 400 == 0)
        {
           // Game1.SpawnProjectile(this.Position, Player.Instance.Position, true); // true means special attack?   
        }
        else
        {
            // melee
            Player.Instance.health -= 20;
        }
    }
}