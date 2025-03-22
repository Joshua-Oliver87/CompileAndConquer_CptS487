using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public abstract class Entity
{
    // Textures/Size
    protected Texture2D texture;
    protected Color color = Color.White;
    public float Scale = 1.0f;
    
    // Position/Movement
    public Vector2 Position;
    public Vector2 Velocity;
    public float Orientation;

    // Collisions
    public bool Collidable = false;
    public bool ColliderDebugMode = false;
    public Rectangle CollisionRectangle;

    // Entity Information
    public int health;
    
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
        
        if (ColliderDebugMode && Collidable)
        {
            Texture2D _texture;
            _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.DarkGreen });
        
            spriteBatch.Draw(_texture,
                CollisionRectangle,
                Color.White * 0.5f);
        }
    }

    public abstract void Update();

    // Collision methods
    public void UpdateCollider()
    {
        // Check if the entity should be collidable and then update the collider rectangle
        if (Collidable)
        {
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, texture.Width * (int)Scale, texture.Height * (int)Scale);
            
            CollisionRectangle.Width = (int)(texture.Width * (Scale));
            CollisionRectangle.Height = (int)(texture.Height * (Scale));
            
            // Update collider
            CollisionRectangle.X = (int)(Position.X - (texture.Width / 2 * (Scale)));
            CollisionRectangle.Y = (int)(Position.Y - (texture.Height / 2 * (Scale)));
        }
    }

    public virtual void OnCollide(Entity collision)
    {
        Console.WriteLine(collision.GetType());
    }
}