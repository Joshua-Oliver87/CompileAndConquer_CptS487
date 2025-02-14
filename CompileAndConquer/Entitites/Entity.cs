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
    float ballSpeed;

    public int health;
    public float Scale = 1.0f;

    public Collider Collider;
    
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
    }

    public abstract void Update();
}