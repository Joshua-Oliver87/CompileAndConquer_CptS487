using System;
using System.Numerics;
using CompileAndConquer.Content;
using CompileAndConquer.Screens;
using Microsoft.Xna.Framework.Input;

namespace CompileAndConquer;

public class InputHandler
{
    // Keyboard references
    private static KeyboardState kstate;
    private static KeyboardState prevkstate;

    // Player instance
    private Player player = Player.Instance;

    /*
    Keybindings
    */
    
    // Movement
    private static Keys up = Keys.Up;
    private static Keys down = Keys.Down;
    private static Keys left = Keys.Left;
    private static Keys right = Keys.Right;
    private static Keys altUp = Keys.W;
    private static Keys altDown = Keys.S;
    private static Keys altLeft = Keys.A;
    private static Keys altRight = Keys.D;
    
    // Controls
    private static Keys slowMode = Keys.LeftShift;
    private static Keys altSlowMode = Keys.LeftControl;
    private static Keys fire = Keys.Space;
    
    // Menu
    private static Keys escape = Keys.Escape;
    
    public static void Update()
    {
        prevkstate = kstate;
        kstate = Keyboard.GetState();
    }

    public static Vector2 GetMovement()
    {
        Vector2 movement = new Vector2();
        
        if (kstate.IsKeyDown(up) || kstate.IsKeyDown(altUp)) 
            movement.Y -= 1;
        if (kstate.IsKeyDown(down) || kstate.IsKeyDown(altDown)) 
            movement.Y += 1;
        if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(altLeft)) 
            movement.X -= 1;
        if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(altRight)) 
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
        if (kstate.IsKeyDown(escape)) // update the pause state if escape pressed
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

    public static int HandleScreenInput(Screen screen, ref int selectedOption)
    {
        string[] menuOptions = screen.MenuOptions;
        
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
      //  return 0;
    }
    
    public static bool GetFire() { return kstate.IsKeyDown(fire); }
    
    public static bool GetSlowMode() { return (kstate.IsKeyDown(slowMode) || kstate.IsKeyDown(altSlowMode)); }
}