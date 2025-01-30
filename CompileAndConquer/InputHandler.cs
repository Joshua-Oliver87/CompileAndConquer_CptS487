using System;
using System.Numerics;
using Microsoft.Xna.Framework.Input;

namespace CompileAndConquer;

public class InputHandler
{
    private static KeyboardState kstate;

    public static void Update()
    {
        kstate = Keyboard.GetState();
    }

    public static Vector2 GetMovement()
    {
        Vector2 movement = new Vector2();
        
        if (kstate.IsKeyDown(Keys.Up))
        {
            movement.Y -= 1;
        }
        
        if (kstate.IsKeyDown(Keys.Down))
        {
            movement.Y += 1;
        }
        
        if (kstate.IsKeyDown(Keys.Left))
        {
            movement.X -= 1;
        }
        
        if (kstate.IsKeyDown(Keys.Right))
        {
            movement.X += 1;
        }
        
        // TODO normalize movement vector

        return movement;
    }
}