using System.Data;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content;

public class EnemyBullet : Entity
{
    public EnemyBullet(Vector2 position, Vector2 velocity)
    {
        this.texture = TextureHandler.Bullet; // Make sure Bullet texture exists in TextureHandler
        this.Position = position;
        this.Velocity = velocity;
        Scale = 2.0f;
        health = 1;
        Collidable = true;
    }

    public override void Update(GameTime gameTime)
    {
        Position += Velocity;
    }
    
    public override void OnCollide(Entity collision)
    {
        // Delete bullet on collision with player
        if (collision is Player)
            this.isAlive = false;
    }
}