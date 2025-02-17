using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace CompileAndConquer;

public class CompileAndConquer : Game
{
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
        
        ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        EntityHandler.Add(Player.Instance);
        SpawnNextEnemy();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        InputHandler.Update();
        EntityHandler.Update();
        
        spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (spawnTimer >= 10)  // Every 10 seconds cycle to next enemy
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
                EntityHandler.AddEntity(new TypeA(TextureHandler.TypeAEnemy, new Vector2(300, 200)));
                break;
            case 1:
                EntityHandler.AddEntity(new TypeB(TextureHandler.TypeBEnemy, new Vector2(500, 500)));
                break;
            case 2:
                EntityHandler.AddEntity(new MiniBoss(TextureHandler.MiniBoss, new Vector2(ScreenSize.X / 2, 100)));
                break;
            case 3:
                EntityHandler.AddEntity(new FinalBoss(TextureHandler.FinalBoss, new Vector2(ScreenSize.X / 2, 100)));
                break;
        }

        spawnPhase = (spawnPhase + 1) % 4;  // Loop back to TypeA after FinalBoss
    }

    private void DespawnAllEnemies()
    {

        foreach (var entity in EntityHandler.Entities.ToList()) 
        {
            if (!(entity is Player)) 
                EntityHandler.RemoveEntity(entity);
        }
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
