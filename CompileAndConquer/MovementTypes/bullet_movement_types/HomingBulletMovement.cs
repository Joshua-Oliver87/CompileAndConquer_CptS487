using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CompileAndConquer.Content;
public class HomingBullet : IMovementStrategy
{
    public void Move(Entity bullet, GameTime gameTime)
    {
        Vector2 target = Player.Instance.Position;
        Vector2 direction = target - bullet.Position;

        if (direction.LengthSquared() > 1f)
            direction.Normalize();

        float speed = 300f;
        bullet.Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}