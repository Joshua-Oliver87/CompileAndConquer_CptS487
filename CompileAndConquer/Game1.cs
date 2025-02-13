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
    
	private double spawnTimer = 0;
	private bool spawningTypeA = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

		_graphics.PreferredBackBufferWidth = 1024;
		_graphics.PreferredBackBufferHeight = 768;
		_graphics.ApplyChanges();
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

        EntityHandler.AddEntity(Player.Instance);
        
        // spawn enemies
		
		SpawnTypeAEnemies();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        InputHandler.Update();
        EntityHandler.Update();
     	
		spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

		if (spawnTimer >= 10){
			spawnTimer = 0;

			if (spawningTypeA) {
				DespawnEnemies();
				SpawnTypeBEnemies();
				spawningTypeA = false;
			}
			else
			{
				DespawnEnemies();
				SpawnTypeAEnemies();
				spawningTypeA = true;
			}
		}
        
        base.Update(gameTime);
    }

	private void SpawnTypeAEnemies()
	{
		Console.WriteLine("Spawning Type A Enemies");
		EntityHandler.AddEntity(new TypeA(TextureHandler.TypeAEnemy, new Vector2(300, 200)));
	}

	private void SpawnTypeBEnemies()
	{
		Console.WriteLine("Spawning Type B Enemies");
		EntityHandler.AddEntity(new TypeB(TextureHandler.TypeBEnemy, new Vector2(500, 500)));
	}
	
	private void DespawnEnemies(){
		Console.WriteLine("Despawning Enemies");
		EntityHandler.ClearEnemies();
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