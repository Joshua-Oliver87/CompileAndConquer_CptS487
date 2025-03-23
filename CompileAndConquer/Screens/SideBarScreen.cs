using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Content;

public class SideBarScreen
{
    private static SideBarScreen instance;
    private Rectangle sideBarRectangle;
    private Texture2D starTexture;
    protected SpriteFont font;

    private int score;
    private int lives;

    private Texture2D PeruBackground;

    public void Initialize(SpriteFont font, GraphicsDevice graphicsDevice, int lives)
    {
        this.font = font;
        this.score = 0;
        this.lives = lives;
        starTexture = TextureHandler.SideBarStar;
        PeruBackground = new Texture2D(graphicsDevice, 1, 1);
        PeruBackground.SetData(new[] { Color.Peru }); 
        this.sideBarRectangle = new Rectangle(0, 0, graphicsDevice.Viewport.Width/4, graphicsDevice.Viewport.Height);
    }

    public void Initialize(SpriteFont font, GraphicsDevice graphicsDevice)
    {
        this.font = font;
        this.score = 0;
        this.lives = 0;
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

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(PeruBackground, sideBarRectangle, Color.Peru);
        spriteBatch.DrawString(font, $"SCORE: {score}", new Vector2(10, 10), Color.Black);
        for (int i = 0; i < lives; i++)
        {
            spriteBatch.Draw(TextureHandler.SideBarStar, new Vector2(15 + i * 45, 45), Color.White);
        }
        spriteBatch.End();
    }

    // public void Update(int lives, int score)
    // {
    //     this.lives = lives;
    //     this.score = score;
    // }

    public void LoseLife()
    {lives = lives -1;}
    
}