using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CompileAndConquer;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    //Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        this.Content.RootDirectory = "Content";
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        TextureHandler.Load(Content);

        EntityHandler.Add(Player.Instance);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        InputHandler.Update();
        EntityHandler.Update();

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