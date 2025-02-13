using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;


namespace CompileAndConquer.Entities.Enemy_Subtypes;

// melee enemy
public class TypeA : Enemy
{
    public TypeA(Texture2D texture, Vector2 position) : base(texture, position, 2.5f)
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
    }

    public override void Attack()
    {
        Player.Instance.health -= 10;   // example
    }
}