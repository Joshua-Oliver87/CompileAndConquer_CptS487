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
        health = 1;
        Damage = 10;
        Collidable = true;
    }

    public override void Update()
    {
        Position += Velocity;
    }

    public override void OnCollide(Entity collision)
    {
        // Delete bullet on collision with enemy
        if (collision is Enemy)
            this.isAlive = false;
    }
}