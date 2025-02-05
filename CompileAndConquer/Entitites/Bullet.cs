using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content;

public class Bullet : Entity
{
    public Bullet(Vector2 position, Vector2 velocity)
    {
        texture = TextureHandler.Bullet;
        this.Position = position;
        this.Velocity = velocity;
    }

    public override void Update()
    {
        Position += Velocity;
    }
}