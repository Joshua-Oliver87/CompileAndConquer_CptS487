using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Content;

public class Player : Entity
{
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
        const float speed = 4.0f;
        Velocity += speed * InputHandler.GetMovement();
        this.Position += Velocity;
        Velocity = Vector2.Zero;
    }
}