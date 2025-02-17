using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;
public abstract class Enemy : Entity
{
    public float Speed { get; protected set; } = 2.0f;    //default speed for enemies
    protected float ScaleFactor = 1.0f; // default

    public Enemy(Texture2D texture, Vector2 position, float speed, float scalePercentage)
    {
        this.texture = texture;
        this.Position = position;
        this.Speed = speed;
        this.health = 100; // default
        
        // ScaleFactor for enemies relative to player size
        this.ScaleFactor = scalePercentage * (Player.Instance.Size.X / texture.Width);
        
        // Collision logic to change later
        CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, texture.Width * (int)ScaleFactor, texture.Height * (int)ScaleFactor);
        DebugMode = true;
        Collidable = true;
        health = 1;
    }

    public abstract override void Update();
    
    public abstract void Attack();
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Position, null, color, Orientation, Size / 2f, ScaleFactor, SpriteEffects.None, 0);
        
        // Collision drawing
        if (DebugMode && Collidable)
        {
            Texture2D _texture;
            _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.DarkGreen });

            spriteBatch.Draw(_texture,
                CollisionRectangle,
                Color.White * 0.5f);
        }
    }
}