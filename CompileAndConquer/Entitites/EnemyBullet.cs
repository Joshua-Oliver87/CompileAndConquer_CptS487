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

        Damage = 10;
    }

    public override void Update()
    {
        Position += Velocity;
    }
}