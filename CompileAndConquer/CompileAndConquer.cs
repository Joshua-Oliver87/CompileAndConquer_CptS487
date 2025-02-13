using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CompileAndConquer;

public class CompileAndConquer : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static Vector2 ScreenSize;

    public CompileAndConquer()
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
        
        // Set our screen size
        ScreenSize.X = GraphicsDevice.Viewport.Width;
        ScreenSize.Y = GraphicsDevice.Viewport.Height;

        EntityHandler.Add(Player.Instance);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // Update our screen size
        ScreenSize.X = GraphicsDevice.Viewport.Width;
        ScreenSize.Y = GraphicsDevice.Viewport.Height;

        // TODO: Add your update logic here
        InputHandler.Update();
        EntityHandler.Update();
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        
        EntityHandler.Draw(_spriteBatch);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}