using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CompileAndConquer;

public class Game1 : Game
{
    public static Random RandomGenerator = new Random();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

		_graphics.PreferredBackBufferWidth = 1024;
		_graphics.PreferredBackBufferHeight = 768;
		_graphics.ApplyChanges();
    }

	// default screen size: W = 800, H = 480
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        this.Content.RootDirectory = "Content";
        
        base.Initialize();
		Console.WriteLine($"Screen Width: {_graphics.PreferredBackBufferWidth}");
    	Console.WriteLine($"Screen Height: {_graphics.PreferredBackBufferHeight}");
    }

    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        TextureHandler.Load(Content);

        EntityHandler.AddEntity(Player.Instance);
        
        // spawn random enemies

        EntityHandler.AddEntity(new TypeA(TextureHandler.TypeAEnemy, new Vector2(300, 200)));
        
        EntityHandler.AddEntity(new TypeB(TextureHandler.TypeBEnemy, new Vector2(500, 300)));
        
        //EntityHandler.AddEntity(new MiniBoss(TextureHandler.Enemy, new Vector2(700, 400)));
        
        //EntityHandler.AddEntity(new FinalBoss(TextureHandler.Enemy, new Vector2(900, 500)));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        InputHandler.Update();
        EntityHandler.Update();
        // The time since Update was called last.
        //float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        

        // if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
        // {
        //     ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
        // }
        // else if (ballPosition.X < ballTexture.Width / 2)
        // {
        //     ballPosition.X = ballTexture.Width / 2;
        // }
        //
        // if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
        // {
        //     ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
        // }
        // else if (ballPosition.Y < ballTexture.Height / 2)
        // {
        //     ballPosition.Y = ballTexture.Height / 2;
        // }
        
        foreach (var entity in EntityHandler.Entities)
            {
                if (entity is Enemy enemy)
                {
                    Console.WriteLine($"Enemy at {enemy.Position}");
                }
            }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        
        EntityHandler.Draw(_spriteBatch);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}