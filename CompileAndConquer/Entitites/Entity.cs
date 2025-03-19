using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public abstract class Entity
{
    protected Texture2D texture;
    protected Color color = Color.White;
    
    public Vector2 Position;
    public Vector2 Velocity;
    public float Orientation;

    public int health;
    public float Scale = 1.0f;

    // Collisions
    public bool Collidable = false;
    public Rectangle CollisionRectangle;
    public bool DebugMode = false;
    
    private Vector2 size
    {
        get
        {
            return texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height);
        }
    }

    public Vector2 Size
    {
        get
        {
            return (size) * Scale;
        }
    }
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Position, null, color, Orientation, size / 2f, Scale, 0, 0);
        
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

    public abstract void Update(GameTime gameTime);
}