using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;
public abstract class Enemy : Entity
{
    public float Speed { get; protected set; } = 2.0f;    //default speed for enemies

    public Enemy(Texture2D texture, Vector2 position, float speed, float scalePercentage)
    {
        this.texture = texture;
        this.Position = position;
        this.Speed = speed;
        this.health = 100; // default
        Damage = 10;
        
        Collidable = true;
        
        // ScaleFactor for enemies relative to player size
        this.Scale = scalePercentage * (Player.Instance.Size.X / texture.Width);
        
        health = 1;
    }

    public abstract override void Update();
    
    public abstract void Attack();
}