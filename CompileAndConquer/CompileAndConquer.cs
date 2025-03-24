using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using CompileAndConquer.Content.WaveManager;

namespace CompileAndConquer;

public class CompileAndConquer : Game
{
    private WaveManager waveManager = new WaveManager();
    public static Random RandomGenerator = new Random();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private double spawnTimer = 0;
    private int spawnPhase = 0;

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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureHandler.Load(Content);

        this.waveManager.LoadWaves();

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

        this.waveManager.UpdateWave(gameTime);

        ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        InputHandler.Update(gameTime);
        AISystem.Update(gameTime); // AI logic
        EntityHandler.Update(gameTime);

        spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (spawnTimer >= 10)  // Every 10 seconds cycle to next enemy
        {
            spawnTimer = 0;
            DespawnAllEnemies();
        }

        base.Update(gameTime);
    }

    /*
    private void SpawnNextEnemy()
    {
        switch (spawnPhase)
        {
            case 0:
                var enemyA = new TypeA(TextureHandler.TypeAEnemy, new Vector2(300, 200), new CurvedMovement());
                EntityHandler.Add(enemyA);
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
    */

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

        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        EntityHandler.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
