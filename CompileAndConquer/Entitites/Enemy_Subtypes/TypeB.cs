using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

// ranged enemy
public class TypeB : Enemy
{
    private bool movingRight = false
    public TypeB(Texture2D texture, Vector2 position) : base(texture, position, 2.5f)
    {
        this.health = 120; // default
    }

    // zigzag pattern
    public override void Update()
    {
        if (movingRight)
        { 
            this.Position.X += Speed;   
        }
        else
        {
            this.Position.X -= Speed;
        }
        
        // switch at boundaries
        if (this.Position.X < 500 || this.Position.X > 100)
            movingRight = !movingRight;
    }

    public override void Attack()
    {
        Game1.SpawnProjectile(this.Position, Player.Instance.Position);
    }
}