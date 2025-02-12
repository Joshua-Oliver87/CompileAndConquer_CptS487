using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;
public abstract class Enemy : Entity
{
    public float Speed { get; protected set; } = 2.0f;    //default speed for enemies

    public TypeA(Texture2D texture, Vector2 position, float speed)
    {
        this.texture = texture;
        this.position = position;
        this.Speed = speed;
        this.health = 100; // default
    }

    public abstract override void Update();

    // add default attack behavior
    public virtual void Attack();
}