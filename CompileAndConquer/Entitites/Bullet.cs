using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content;

public class Bullet : Entity
{
    public Bullet(Vector2 position, Vector2 velocity)
    {
        texture = TextureHandler.Bullet;
        this.Position = position;
        this.Velocity = velocity;
        Scale = 2.0f;
    }

    public override void Update()
    {
        Position += Velocity;
    }
}