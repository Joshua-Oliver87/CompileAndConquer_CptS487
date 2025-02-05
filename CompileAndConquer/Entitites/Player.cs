using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Content;

public class Player : Entity
{

    // 
    private int bulletCooldown = 0;
    private int maxBulletCooldown = 12;
    
    
    private float speed = 4.0f;
    
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
    // Uncopied
    
    private Player()
    {
        texture = TextureHandler.Player;
        Console.WriteLine(CompileAndConquer.ScreenSize);
        Position = new Vector2(CompileAndConquer.ScreenSize.X / 2, CompileAndConquer.ScreenSize.Y / 2);
        Scale = 2.0f;
    }

    public override void Update()
    {
        // Iterate bulletCooldown down
        if (bulletCooldown > 0)
            bulletCooldown--;


        Velocity += speed * InputHandler.GetMovement(); 
        this.Position += Velocity;

        // Check bounds
        if (this.Position.X - this.Size.X / 2 < 0)
            this.Position.X = 0 + this.Size.X / 2;
        if (this.Position.Y - this.Size.Y / 2 < 0)
            this.Position.Y = 0 + this.Size.Y / 2;
        if (this.Position.X + this.Size.X / 2 > CompileAndConquer.ScreenSize.X)
            this.Position.X = CompileAndConquer.ScreenSize.X - this.Size.X / 2;
        if (this.Position.Y + this.Size.Y / 2 > CompileAndConquer.ScreenSize.Y)
            this.Position.Y = CompileAndConquer.ScreenSize.Y - this.Size.Y / 2;
        
        Velocity = Vector2.Zero;

        if (InputHandler.GetFire() && bulletCooldown == 0)
        {
            EntityHandler.Add(new Bullet(this.Position + new Vector2(0, -50), new Vector2(0, -10.0f)));
            bulletCooldown = maxBulletCooldown;
        }
    }
}