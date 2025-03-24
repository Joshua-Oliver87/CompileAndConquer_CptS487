using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using CompileAndConquer.Screens;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Content;

public class Player : Entity
{

    // 
    private int bulletCooldown = 0;
    private int maxBulletCooldown = 12;
    
    
    private float speed = 4.0f;

    public int Lives = 3;

    public Action OnLoseLife { get; set; }

    public bool Invincible = false;
    
    private int invincibleTimer = 0;
    private int invincibleDuration = 3;
    
    private int damage = 10;
    
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
        if (bulletCooldown == 0)
        {
            EntityHandler.Add(new PlayerBullet(this.Position + new Vector2(22, -18), new Vector2(0, -10.0f), damage));
            EntityHandler.Add(new PlayerBullet(this.Position + new Vector2(-22, -18), new Vector2(0, -10.0f), damage));
            bulletCooldown = maxBulletCooldown;
        }
    }

    public override void Update()
    {
        // Iterate bulletCooldown down
        if (bulletCooldown > 0)
            bulletCooldown--;
    }

    public void LoseLife()
    {
        if (Invincible) return;
        
        if (Lives > 0)
        {
            Lives--;
            
            // respawn from center bottom screen
            // invincible for a few seconds
            // clear bullets
            OnLoseLife?.Invoke();

            Invincible = true;
            invincibleTimer = invincibleDuration;
            Position = new Vector2(CompileAndConquer.ScreenSize.X / 2, CompileAndConquer.ScreenSize.Y - 100);
            EntityHandler.RemoveAllEnemyBullets();

        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        Console.WriteLine("Player has been defeated");
        
        // show end screen
        EndScreen.Instance.gameEnd = true;
        EndScreen.Instance.Update();
    }
}