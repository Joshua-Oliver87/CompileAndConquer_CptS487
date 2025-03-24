using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using CompileAndConquer.Screens;

namespace CompileAndConquer;

public class CompileAndConquer : Game
{
    private KeyboardState prevKeyboardState;

    public static Random RandomGenerator = new Random();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public SpriteFont screenFont { get; private set; }

    private double spawnTimer = 0;
    private int spawnPhase = 0;


    //REPLACE WITH SCREEN HANDLER
    private readonly StartScreen startScreen = StartScreen.Instance;
    private readonly PauseMenuScreen pauseScreen = PauseMenuScreen.Instance;
    private readonly EndScreen endScreen = EndScreen.Instance;
    private readonly SideBarScreen sidebar= SideBarScreen.Instance;

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
        Content.RootDirectory = "Content";

        PauseMenuScreen.Instance.OnExitGame = Exit;
        EndScreen.Instance.OnExitGame = Exit;
        StartScreen.Instance.OnExitGame = Exit;

       //Player.Instance.OnScoreUpdate = UpdateScore(score);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureHandler.Load(Content);

        Player.Instance.OnLoseLife = LoseLife;

        screenFont = Content.Load<SpriteFont>("MenuFont");
        //replace with screen manager screenmanager
        StartScreen.Instance.Initialize(screenFont);

        PauseMenuScreen.Instance.Initialize(screenFont);
        EndScreen.Instance.Initialize(screenFont);
   //     SideBarScreen.Instance.Initialize(screenFont, GraphicsDevice);

        ScreenSize.X = GraphicsDevice.Viewport.Width;
        ScreenSize.Y = GraphicsDevice.Viewport.Height;

        EntityHandler.Add(Player.Instance);
        Player.Instance.Position = new Vector2(ScreenSize.X / 2, ScreenSize.Y / 1.25f);
        SpawnTypeAEnemies();
    }


    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape)&& prevKeyboardState.IsKeyUp(Keys.Escape))
        {
            PauseMenuScreen.Instance.isPaused = !PauseMenuScreen.Instance.isPaused;
        }
        
        prevKeyboardState = keyboardState;

        InputHandler.Update();

        if (StartScreen.Instance.gameStart == false)
        {
            startScreen.Update();
            base.Update(gameTime);

            return;
        }
        

        if (PauseMenuScreen.Instance.isPaused)
        {
            pauseScreen.Update();
            base.Update(gameTime);

            return; 
        }

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keyboardState.IsKeyDown(Keys.Delete))
        {
            Exit();
        }

        ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        EntityHandler.Update();

        spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (spawnTimer >= 10) 
        {
            spawnTimer = 0;
            DespawnAllEnemies();
            SpawnNextEnemy();
        }

        // game over (survived)
        if (spawnPhase == 4)
        {
            endScreen.gameEnd = true;
            endScreen.Update();
            base.Update(gameTime);

            return;
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

        // spawnPhase = (spawnPhase + 1) % 4;  // Loop back to TypeA after FinalBoss
        spawnPhase = (spawnPhase + 1) % 5; // logic for end game
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

        if (PauseMenuScreen.Instance.isPaused)
        {
            PauseMenuScreen.Instance.Draw(_spriteBatch);
        }
        else if (EndScreen.Instance.gameEnd)
        {
            EndScreen.Instance.Draw(_spriteBatch);
        }
        else if (!StartScreen.Instance.gameStart)
        {
            StartScreen.Instance.Draw(_spriteBatch);
        }
        else
        {
            EntityHandler.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    protected void LoseLife()
    {
        SideBarScreen.Instance.LoseLife();
    }

    // protected void UpdateScore(int score)
    // {
    //     SideBarScreen.Instance.UpdateScore(score);
    // }
}