using CompileAndConquer.Content;
using CompileAndConquer.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace CompileAndConquer;

public class CompileAndConquer : Game
{
    //private static bool isPaused = false; //if the game is paused, do not update or redraw.

    public static Random RandomGenerator = new Random();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public SpriteFont screenFont { get; private set; }

    private double spawnTimer = 0;
    private int spawnPhase = 0;
    
    ///private SpriteFont _menuFont;
    // private string[] _menuOptions = { "Resume", "Quit" };
    // private int _selectedOption = 0;

    //REPLACE WITH SCREEN HANDLER
    private readonly PauseMenuScreen pauseScreen = PauseMenuScreen.Instance;


    public static Vector2 ScreenSize;

    public CompileAndConquer()
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
        this.Content.RootDirectory = "Content";
        PauseMenuScreen.Instance.OnExitGame = Exit;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureHandler.Load(Content);

        screenFont = Content.Load<SpriteFont>("MenuFont");
        PauseMenuScreen.Instance.Initialize(screenFont);


        ScreenSize.X = GraphicsDevice.Viewport.Width;
        ScreenSize.Y = GraphicsDevice.Viewport.Height;

        EntityHandler.Add(Player.Instance);
        SpawnTypeAEnemies();
    }


    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape))
        {
            // _showPauseMenu = !_showPauseMenu;
            // isPaused = _showPauseMenu;
            //isPaused = !isPaused;
            PauseMenuScreen.Instance.isPaused = !PauseMenuScreen.Instance.isPaused;
        }
        
        //if (_showPauseMenu)
        if (PauseMenuScreen.Instance.isPaused)
        {
            pauseScreen.Update();
            return; 
        }

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keyboardState.IsKeyDown(Keys.Delete))
        {
            Exit();
        }

        ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        InputHandler.Update();
        EntityHandler.Update();

        spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (spawnTimer >= 10) 
        {
            spawnTimer = 0;
            DespawnAllEnemies();
            SpawnNextEnemy();
        }

        base.Update(gameTime);
    }


    private void SpawnNextEnemy()
    {
        switch (spawnPhase)
        {
            case 0:
                EntityHandler.Add(new TypeA(TextureHandler.TypeAEnemy, new Vector2(300, 200)));
                break;
            case 1:
                EntityHandler.Add(new TypeB(TextureHandler.TypeBEnemy, new Vector2(500, 500)));
                break;
            case 2:
                EntityHandler.Add(new MiniBoss(TextureHandler.MiniBoss, new Vector2(ScreenSize.X / 2, 100)));
                break;
            case 3:
                EntityHandler.Add(new FinalBoss(TextureHandler.FinalBoss, new Vector2(ScreenSize.X / 2, 100)));
                break;
        }

        spawnPhase = (spawnPhase + 1) % 4;  // Loop back to TypeA after FinalBoss
    }

    private void DespawnAllEnemies()
    {

        foreach (var entity in EntityHandler.entities.ToList()) 
        {
            if (!(entity is Player)) 
                EntityHandler.RemoveEntity(entity);
        }
    }
	private void SpawnTypeAEnemies()
	{
		Console.WriteLine("Spawning Type A Enemies");
		EntityHandler.Add(new TypeA(TextureHandler.TypeAEnemy, new Vector2(300, 200)));
	}

	private void SpawnTypeBEnemies()
	{
		Console.WriteLine("Spawning Type B Enemies");
		EntityHandler.Add(new TypeB(TextureHandler.TypeBEnemy, new Vector2(500, 500)));
	}
	
	private void DespawnEnemies(){
		Console.WriteLine("Despawning Enemies");
		EntityHandler.ClearEnemies();
	}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        // Add drawing code here

        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

        if (screenFont != null)
        {
            //spriteBatch.DrawString(font, "Paused", new Vector2(100, 100), Color.White);
        }
        else
        {
            Console.WriteLine("Font is null!");
        }

        //if (_showPauseMenu) //if not paused then draw
        if (PauseMenuScreen.Instance.isPaused)
        {
            
            PauseMenuScreen.Instance.Draw(_spriteBatch);
        }
        else
        {
            EntityHandler.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

}