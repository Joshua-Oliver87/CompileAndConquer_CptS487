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

    private float screenWidth;
    private float screenHeight;
    private Texture2D PeruBackground;

    public void Initialize(SpriteFont font, GraphicsDevice graphicsDevice, int lives = 10)
    {
        this.font = font;
        this.score = 0;
        this.lives = lives;
        starTexture = TextureHandler.SideBarStar;
        PeruBackground = new Texture2D(graphicsDevice, 1, 1);
        PeruBackground.SetData([Color.Peru]); 
        screenWidth = graphicsDevice.Viewport.Width;
        screenHeight = graphicsDevice.Viewport.Height;
        this.sideBarRectangle = new Rectangle(0, 0, (int)screenWidth/4, (int)screenHeight);
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
        if (PeruBackground == null)
        {
            Console.WriteLine("background texture is null");
        }
        else
        {
            spriteBatch.Draw(PeruBackground, sideBarRectangle, Color.Peru);
            spriteBatch.DrawString(font, $"SCORE: {score}", new Vector2(10, 10), Color.Black);
        }
        if (TextureHandler.SideBarStar == null)
        {
            Console.WriteLine("star texture is null");
        }
        else
        {
            Vector2 basePosition = new Vector2(screenWidth * 0.03f, screenHeight * 0.1f); 
            float spacing = screenWidth * 0.05f;
            float scale = ((screenWidth / 50) / basePosition.X/2 ) * 0.5f;  
            int col = 0;
            for (int i = 0; i < lives; i++)
            {
                if(i % 4 == 0)
                {
                    basePosition.Y = screenHeight * 0.1f + (10 * i);
                    col = 0;
                }
                Vector2 position = new Vector2(basePosition.X + col * spacing, basePosition.Y);
                spriteBatch.Draw(starTexture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                col += 1;
            }
        }

    }

    // public void Update(int lives, int score)
    // {
    //     this.lives = lives;
    //     this.score = score;
    // }

    public void LoseLife()
    {
        lives = lives - 1;
    }

    public void UpdateScore(int score)
    {
    this.score = score;
    }
    
}