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
        Collider = new Collider(texture.Height * (int)Scale, texture.Width * (int)Scale, this);
    }

    public override void Update()
    {
        Position += Velocity;
    }
}