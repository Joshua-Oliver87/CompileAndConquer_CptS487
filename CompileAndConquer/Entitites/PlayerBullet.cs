using System;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content;

public class PlayerBullet : Entity
{
    public PlayerBullet(Vector2 position, Vector2 velocity)
    {
        this.texture = TextureHandler.Bullet;
        this.Position = position;
        this.Velocity = velocity;
        Scale = 2.0f;
        
        CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, texture.Width * (int)Scale, texture.Height * (int)Scale);
        DebugMode = false;
        Collidable = true;
        health = 1;
    }

    public override void Update()
    {
        Position += Velocity;
        
        // Update collider
        CollisionRectangle.X = (int)Position.X - (texture.Width / 2 * (int)Scale);
        CollisionRectangle.Y = (int)Position.Y - (texture.Height / 2 * (int)Scale);
    }
}