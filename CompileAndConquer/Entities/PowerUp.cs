using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public class PowerUp : Entity
{
    public PowerUp(Vector2 position)
    {
        this.texture = TextureHandler.PowerUp;
        this.Position = position;
        this.Scale = 1.0f;
        this.Collidable = true;
        this.ColliderScale = 0.5f;
        this.ColliderScalingSeparate = true;
    }

    public override void Update(GameTime gameTime)
    {
        this.Position += new Vector2(0, 1.5f);

        if (this.Position.Y > CompileAndConquer.ScreenSize.Y + Size.Y)
        {
            this.isAlive = false;
            EntityHandler.RemoveEntity(this);
        }
    }

    public override void OnCollide(Entity collision)
    {
        if (collision is Player player)
        {
            player.ActivatePowerup();
            this.isAlive = false;
            EntityHandler.RemoveEntity(this);
        }
    }
}