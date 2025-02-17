using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

// melee enemy
public class TypeA : Enemy
{
    public TypeA(Texture2D texture, Vector2 position) : base(texture, position, 2.5f, 0.5f)
    {
        this.health = 80; // default
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
    }

    public override void Attack()
    {
        Player.Instance.health -= 10;   // example
    }

}