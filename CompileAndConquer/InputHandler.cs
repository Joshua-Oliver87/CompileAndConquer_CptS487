using System;
using System.Numerics;
using Microsoft.Xna.Framework.Input;

namespace CompileAndConquer;

public class InputHandler
{
    private static KeyboardState kstate;
    private static KeyboardState prevkstate;

    public static void Update()
    {
        prevkstate = kstate;
        kstate = Keyboard.GetState();
    }

    public static Vector2 GetMovement()
    {
        Vector2 movement = new Vector2();
        
        if (kstate.IsKeyDown(Keys.Up)) 
            movement.Y -= 1;
        if (kstate.IsKeyDown(Keys.Down)) 
            movement.Y += 1;
        if (kstate.IsKeyDown(Keys.Left)) 
            movement.X -= 1;
        if (kstate.IsKeyDown(Keys.Right)) 
            movement.X += 1;
        
        // TODO normalize movement vector
        if (movement.Length() > 0)
        {
            movement = Vector2.Normalize(movement);
        }

        return movement;
    }

    public static bool PauseGame()
    {
        bool isPaused = false;
        if (kstate.IsKeyDown(Keys.Escape)) //update the pause state if escape pressed
        {
            isPaused = true;
        }
        return isPaused;
    }

    public static int HandlePauseMenuInput(ref int selectedOption)
    {
        string[] menuOptions = { "Resume", "Quit" };

        if (kstate.IsKeyDown(Keys.Up))
        {
            selectedOption = (selectedOption - 1 + menuOptions.Length) % menuOptions.Length;
        }
        else if (kstate.IsKeyDown(Keys.Down))
        {
            selectedOption = (selectedOption + 1) % menuOptions.Length;
        }
        else if (kstate.IsKeyDown(Keys.Enter))
        {
            return selectedOption;
        }
        return -1;
    }
    
    public static bool GetFire() { return kstate.IsKeyDown(Keys.Space); }
    
    public static bool GetSlowMode() { return kstate.IsKeyDown(Keys.LeftControl); }
}