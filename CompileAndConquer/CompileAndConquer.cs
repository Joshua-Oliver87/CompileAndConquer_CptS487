using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using CompileAndConquer.Screens;
using CompileAndConquer.Content.WaveManager;

namespace CompileAndConquer;

public class CompileAndConquer : Game
{
    private KeyboardState prevKeyboardState;

    private WaveManager waveManager = new WaveManager();
    public static Random RandomGenerator = new Random();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public SpriteFont screenFont { get; private set; }

    private double spawnTimer = 0;
    private int spawnPhase = 0;


    //REPLACE WITH SCREEN HANDLER
    private readonly StartScreen startScreen =new StartScreen();
    private readonly PauseMenuScreen pauseScreen = new PauseMenuScreen();
    private readonly EndScreen endScreen = new EndScreen();
    private readonly SettingMenuScreen settingScreen = new SettingMenuScreen();
    private readonly SideBarScreen sidebar= SideBarScreen.Instance;
    private readonly KeyBindScreen keyBindScreen = new KeyBindScreen();
    public static int score = 0;

    public static Vector2 ScreenSize;

    private bool death = false;

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

        // PauseMenuScreen.Instance.OnExitGame = Exit;
        // EndScreen.Instance.OnExitGame = Exit;
        // StartScreen.Instance.OnExitGame = Exit;    
        pauseScreen.OnExitGame = Exit;    
        endScreen.OnExitGame = Exit;
        startScreen.OnExitGame =Exit;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureHandler.Load(Content);

        Player.Instance.OnLoseLife = LoseLife;
        Player.Instance.OnDeath = Death;


        screenFont = Content.Load<SpriteFont>("MenuFont");

        startScreen.Initialize(screenFont);
        pauseScreen.Initialize(screenFont);
        endScreen.Initialize(screenFont);
        settingScreen.Initialize(screenFont);
        keyBindScreen.Initialize(screenFont);

        startScreen.SetScreenContext(ScreenContext.Instance);
        pauseScreen.SetScreenContext(ScreenContext.Instance);
        endScreen.SetScreenContext(ScreenContext.Instance);
        settingScreen.SetScreenContext(ScreenContext.Instance);
        keyBindScreen.SetScreenContext(ScreenContext.Instance);

        settingScreen.SetPauseScreen(pauseScreen);
        pauseScreen.SetEndScreen(endScreen);
        pauseScreen.SetSettingScreen(settingScreen);
        startScreen.SetSettingScreen(settingScreen);
        endScreen.SetStartScreen(startScreen);
        settingScreen.SetKeyBindScreen(keyBindScreen);
        keyBindScreen.SetSettingScreen(settingScreen);
        
        ScreenContext.Instance.Initialize(screenFont, startScreen);


        this.waveManager.LoadWaves();
        this.waveManager.Survived = Win;

        // Set our screen size
        ScreenSize.X = GraphicsDevice.Viewport.Width;
        ScreenSize.Y = GraphicsDevice.Viewport.Height;

        EntityHandler.Add(Player.Instance);
        Player.Instance.Position = new Vector2(ScreenSize.X / 2, ScreenSize.Y / 1.25f);
        SideBarScreen.Instance.Initialize(screenFont, GraphicsDevice, Player.Instance.Lives);
    }


    protected override void Update(GameTime gameTime)
    {

        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape)&& prevKeyboardState.IsKeyUp(Keys.Escape))
        {
            ScreenContext.Instance.SetScreen(pauseScreen);
        }
        
        prevKeyboardState = keyboardState;

        InputHandler.Update();

        if (ScreenContext.Instance.isVisible == true)
        {
            ScreenContext.Instance.Update();
            base.Update(gameTime);
            return;
        }

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keyboardState.IsKeyDown(Keys.Delete))
        {
            Exit();
        }
        
        this.waveManager.UpdateWave(gameTime);

        ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        InputHandler.Update();
        AISystem.Update(gameTime); // AI logic
        EntityHandler.Update(gameTime);

        base.Update(gameTime);
    }

    private void DespawnAllEnemies()
    {
        foreach (var entity in EntityHandler.entities.ToList()) 
        {
            if (!(entity is Player)) 
                EntityHandler.RemoveEntity(entity);
        }
    }
    

    private void DespawnEnemies()
    {
        Console.WriteLine("Despawning Enemies");
        EntityHandler.ClearEnemies();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        // Add drawing code here

        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);


        if (ScreenContext.Instance.isVisible == true)
        {
            ScreenContext.Instance.Draw(_spriteBatch);
        }
        else
        {
            this.UpdateScore(CompileAndConquer.score);
            SideBarScreen.Instance.Draw(_spriteBatch);
            EntityHandler.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    protected void Death()
    {
        this.death = true;
        endScreen.win = false;
        EntityHandler.RemoveAll();
        ScreenContext.Instance.SetScreen(endScreen); // TODO possible editing here
    }
    protected void LoseLife()
    {
        SideBarScreen.Instance.LoseLife();
    }



    protected void UpdateScore(int score)
    {
        SideBarScreen.Instance.UpdateScore(score);
    }

    protected void Win()
    {            
        endScreen.win = true;
        ScreenContext.Instance.SetScreen(endScreen);
        endScreen.Update();
    }
}