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
    internal float Scale = 1.0f;
    
    // Position/Movement
    internal Vector2 Position;
    internal Vector2 Velocity;
    internal float Orientation;
    
    // Collisions
    internal bool Collidable = false;
    internal bool ColliderDebugMode = false;
    internal Rectangle CollisionRectangle;
    internal bool ColliderScalingSeparate = false;
    internal float ColliderScale = 1.0f;

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
            // Create collision rectangle
            CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, texture.Width * (int)Scale, texture.Height * (int)Scale);

            // Scale and position it for having separate collider scaling based on the scale of the object * scale of the collider
            if (ColliderScalingSeparate)
            {
                // Set collider size
                CollisionRectangle.Width = (int)(texture.Width * (ColliderScale * Scale));
                CollisionRectangle.Height = (int)(texture.Height * (ColliderScale * Scale));
                
                // Update collider
                CollisionRectangle.X = (int)(Position.X - (texture.Width / 2 * (ColliderScale * Scale)));
                CollisionRectangle.Y = (int)(Position.Y - (texture.Height / 2 * (ColliderScale * Scale)));
            }
            
            // Scale and position it for having the same scale as the entity (matching the size of the sprite)
            else
            {
                // Set collider size
                CollisionRectangle.Width = (int)(texture.Width * (Scale));
                CollisionRectangle.Height = (int)(texture.Height * (Scale));
                
                // Update collider
                CollisionRectangle.X = (int)(Position.X - (texture.Width / 2 * (Scale)));
                CollisionRectangle.Y = (int)(Position.Y - (texture.Height / 2 * (Scale)));
            }
        }
    }

    public virtual void OnCollide(Entity collision)
    {
        Console.WriteLine(collision.GetType());
    }
}