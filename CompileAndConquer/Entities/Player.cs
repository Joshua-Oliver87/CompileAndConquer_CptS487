using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using CompileAndConquer.Screens;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Content;

public class Player : Entity
{
    private int bulletCooldown = 0;
    private int maxBulletCooldown = 12;
    private float speed = 4.0f;
    public int Lives = 30;
    public Action OnLoseLife { get; set; }
    public Action OnDeath { get; set; }
    public bool Invincible = false;
    private int invincibleTimer = 0;
    private int invincibleDuration = 3;
    
    private bool powerupActive = false;
    private double powerupTimer = 0;
    private double powerupDuration = 5;
    private int baseDamage = 10;

    private Stopwatch startTime;
    private Stopwatch invincibilityTimer = new Stopwatch();
    
    private int damage => powerupActive ? baseDamage * 2 : baseDamage;
    
    // Copied
    private static Player instance;
    public static Player Instance 
    {
        get
        {
            if (instance == null)
                instance = new Player();

            return instance;
        }
    }
    // Un-copied
    
    private Player()
    {
        texture = TextureHandler.Player;
        Position = new Vector2(CompileAndConquer.ScreenSize.X / 2, CompileAndConquer.ScreenSize.Y / 2);
        Scale = 2.0f;
        ColliderDebugMode = true;
        Collidable = true;
        health = 100;
        ColliderScale = 0.5f;
    }
    
    // Input/Movement Methods
    public void Move(Vector2 velocity)
    {
        ColliderDebugMode = false;
        ColliderScalingSeparate = false;
        Velocity += speed * velocity;

        this.Position += Velocity;
        
        // Check bounds 
        if (this.Position.X - (this.Size.X / 2) < 0)
            this.Position.X = 0 + (this.Size.X / 2);
        if (this.Position.Y - (this.Size.Y / 2) < 0)
            this.Position.Y = 0 + (this.Size.Y / 2);
        if (this.Position.X + this.Size.X / 2 > CompileAndConquer.ScreenSize.X)
            this.Position.X = CompileAndConquer.ScreenSize.X - this.Size.X / 2;
        if (this.Position.Y + this.Size.Y / 2 > CompileAndConquer.ScreenSize.Y)
            this.Position.Y = CompileAndConquer.ScreenSize.Y - this.Size.Y / 2;
        
        Velocity = Vector2.Zero;
    }
    
    public void MoveSlow(Vector2 velocity)
    {
        ColliderDebugMode = true;
        ColliderScalingSeparate = true;
        Velocity += speed * velocity * 0.5f;

        this.Position += Velocity;
        
        // Check bounds 
        if (this.Position.X - (this.Size.X / 2) < 0)
            this.Position.X = 0 + (this.Size.X / 2);
        if (this.Position.Y - (this.Size.Y / 2) < 0)
            this.Position.Y = 0 + (this.Size.Y / 2);
        if (this.Position.X + this.Size.X / 2 > CompileAndConquer.ScreenSize.X)
            this.Position.X = CompileAndConquer.ScreenSize.X - this.Size.X / 2;
        if (this.Position.Y + this.Size.Y / 2 > CompileAndConquer.ScreenSize.Y)
            this.Position.Y = CompileAndConquer.ScreenSize.Y - this.Size.Y / 2;
        
        Velocity = Vector2.Zero;
    }

    public void Fire()
    {
        // Fire Controls
        if (!powerupActive)
        {
            if (bulletCooldown == 0)
            {
                EntityHandler.Add(new PlayerBullet(this.Position + new Vector2(22, -18), new Vector2(0, -10.0f), damage));
                EntityHandler.Add(new PlayerBullet(this.Position + new Vector2(-22, -18), new Vector2(0, -10.0f), damage));
                bulletCooldown = maxBulletCooldown;
            }
        }
        else
        {
            if (bulletCooldown == 0)
            {
                EntityHandler.Add(new PlayerBullet(this.Position + new Vector2(22, -18), new Vector2(0, -10.0f), damage));
                EntityHandler.Add(new PlayerBullet(this.Position + new Vector2(-22, -18), new Vector2(0, -10.0f), damage));
                EntityHandler.Add(new PlayerBullet(this.Position + new Vector2(44, -18), new Vector2(0, -10.0f), damage));
                EntityHandler.Add(new PlayerBullet(this.Position + new Vector2(-44, -18), new Vector2(0, -10.0f), damage));
                bulletCooldown = maxBulletCooldown;
            }
        }
    }

    public override void OnCollide(Entity collision)
    {
        if (collision is EnemyBullet || collision is Enemy)
        {
            Console.WriteLine("collision");
            LoseLife();
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (Invincible && invincibilityTimer.Elapsed.TotalSeconds >= 3)
        {
            Invincible = false;
            invincibilityTimer.Stop();
        }
        // Iterate bulletCooldown down
        if (bulletCooldown > 0)
            bulletCooldown--;
        
        // powerup active
        if (powerupActive)
        {
            powerupTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (powerupTimer >= powerupDuration)
            {
                powerupActive = false;
            }
        }
    }

    public void LoseLife()
    {
        if (Invincible) return;
        
        OnLoseLife?.Invoke();

        
        if (Lives > 1)
        {
            Lives--;
            Console.WriteLine("lost life");
            
            // respawn from center bottom screen
            // invincible for a few seconds
            // clear bullets

            Invincible = true;
            invincibilityTimer.Restart();
            Position = new Vector2(CompileAndConquer.ScreenSize.X / 2, CompileAndConquer.ScreenSize.Y - 100);
            EntityHandler.RemoveAllEnemyBullets();

        }
        else
        {
            Console.WriteLine("supposedly dead");
            Die();
        }
    }

    public void Die()
    {
        Console.WriteLine("Player has been defeated");

        // remove all entities
        OnDeath?.Invoke();

        // show end screen
        EndScreen.Instance.gameEnd = true;
        EndScreen.Instance.Update();

    }

    public void StartTimer()
    {
        startTime.Start();
    }

    public double GetSurvivalTime() => startTime.Elapsed.TotalSeconds;

    public void ActivatePowerup()
    {
        powerupActive = true;
        powerupTimer = 0.0;
    }
}