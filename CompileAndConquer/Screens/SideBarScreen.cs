using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Content;

public class SideBarScreen : Screen
{
    private static SideBarScreen instance;
    private Rectangle sideBarRectangle;
    private Texture2D starTexture;


    public void Initialize(SpriteFont font, GraphicsDevice graphicsDevice)
    {
        this.font = font;
        starTexture = TextureHandler.SideBarStar;
        this.sideBarRectangle = new Rectangle(0, 0, graphicsDevice.Viewport.Width/4, graphicsDevice.Viewport.Height);
    }

    public static SideBarScreen Instance
    {
        get{
            if (instance == null)
            {
                instance = new SideBarScreen();
            }
            return instance;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
}