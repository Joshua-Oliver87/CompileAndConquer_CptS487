using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public abstract class Entity
{
    protected Texture2D texture;
    protected Color color = Color.White;
    
    public Vector2 Position;
    public Vector2 Velocity;
    public float Orientation;
    float ballSpeed;
    
    public Vector2 Size
    {
        get
        {
            return texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height);
        }
    }
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Position, null, color, Orientation, Size / 2f, 1f, 0, 0);
    }

    public abstract void Update();
}