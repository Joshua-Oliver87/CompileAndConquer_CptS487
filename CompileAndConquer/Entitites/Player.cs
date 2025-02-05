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
    }

    public override void Update()
    {
        // Iterate bulletCooldown down
        if (bulletCooldown > 0)
            bulletCooldown--;
        
        const float speed = 4.0f;
        Velocity += speed * InputHandler.GetMovement();
        this.Position += Velocity;
        Velocity = Vector2.Zero;

        if (InputHandler.GetFire() && bulletCooldown == 0)
        {
            EntityHandler.Add(new Bullet(this.Position + new Vector2(0, -50), new Vector2(0, -10.0f)));
            bulletCooldown = maxBulletCooldown;
        }
    }
}