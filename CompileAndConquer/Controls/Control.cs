using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Control;

public abstract class Control
{
    public string ID;
    public string Text;
    public Vector2 Position;
    protected Texture2D texture;

    public Control()
    {}

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        //spriteBatch.Draw(texture, Position, null, color, , size / 2f, Scale, 0, 0);
        
        // if (DebugMode && Collidable)
        // {
        //     Texture2D _texture;
        //     _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        //     _texture.SetData(new Color[] { Color.DarkGreen });

        //     spriteBatch.Draw(_texture,
        //         CollisionRectangle,
        //         Color.White * 0.5f);
        // }
    }

    public abstract void Update();

}