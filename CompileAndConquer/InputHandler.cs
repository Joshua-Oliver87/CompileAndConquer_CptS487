
using System;
using CompileAndConquer.Content;
using CompileAndConquer.Screens;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace CompileAndConquer;

public class InputHandler
{
    // Keyboard references
    private static KeyboardState kstate;
    private static KeyboardState prevkstate;

    // Player instance
    private static Player player = Player.Instance;

    /*
    Keybindings
    */

    // Movement
    public static Keys up { get; set; } = Keys.Up;
    public static Keys down { get; set; } = Keys.Down;
    public static Keys left { get; set; } = Keys.Left;
    public static Keys right { get; set; } = Keys.Right;
    // public static Keys altUp{get;set;} = Keys.W;
    // public static Keys altDown{get;set;} = Keys.S;
    // public static Keys altLeft{get;set;} = Keys.A;
    // public static Keys altRight{get;set;} = Keys.D;

    // Controls
    public static Keys slowMode { get; set; } = Keys.LeftShift;
    //private static Keys altSlowMode = Keys.LeftControl;
    public static Keys fire { get; set; } = Keys.Space;
    private static Keys cheatModeOn = Keys.N;
    private static Keys cheatModeOff = Keys.M;


    // Menu
    public static Keys escape = Keys.Escape;

    public static void Update()
    {
        prevkstate = kstate;
        kstate = Keyboard.GetState();
        SendPlayerMovement();
        PlayerFire();
        CheatMode();
    }

    private static void SendPlayerMovement()
    {
        Vector2 movement = new Vector2();

        if (kstate.IsKeyDown(up))
            movement.Y -= 1;
        if (kstate.IsKeyDown(down))
            movement.Y += 1;
        if (kstate.IsKeyDown(left))
            movement.X -= 1;
        if (kstate.IsKeyDown(right))
            movement.X += 1;

        // TODO normalize movement vector
        if (movement.Length() > 0)
        {
            movement = Vector2.Normalize(movement);
        }
        if (GetSlowMode())
            player.MoveSlow(movement);

        else
            player.Move(movement);
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

    public static int HandleScreenInput(Screen screen, ref int selectedOption)
    {
        string[] menuOptions = screen.MenuOptions;
        if (kstate.IsKeyDown(Keys.Up) && prevkstate.IsKeyUp(Keys.Up))
        {
            selectedOption = (selectedOption - 1 + menuOptions.Length) % menuOptions.Length;
        }
        else if (kstate.IsKeyDown(Keys.Down) && prevkstate.IsKeyUp(Keys.Down))
        {
            selectedOption = (selectedOption + 1) % menuOptions.Length;
        }
        else if (kstate.IsKeyDown(Keys.Enter) && prevkstate.IsKeyUp(Keys.Enter))
        {
            return selectedOption;
        }
        return -1;
        //  return 0; // for debugging
    }

    public static void PlayerFire()
    {
        if (kstate.IsKeyDown(fire))
            player.Fire();
    }


    public static bool GetSlowMode() { return (kstate.IsKeyDown(slowMode)); }

    public static void CheatMode()
    {
        if (kstate.IsKeyDown(cheatModeOn))
            player.CheatModeOn();

        if (kstate.IsKeyDown(cheatModeOff))
            player.CheatModeOff();
    }


    public static bool setCurrentKey(int i)
    {
        if (kstate.GetPressedKeys().Length == 0)
        {
            return false;
        }
        Keys curkey = kstate.GetPressedKeys()[0];
        // Console.WriteLine(i.ToString()+" : "+curkey.ToString());
        switch (i + 1)
        {
            //"Up","Down", "Right","Left","Slow Mode","Fire","Pause",
            case 1:
                up = curkey;
                break;
            case 2:
                down = curkey;
                break;
            case 3:
                right = curkey;
                break;
            case 4:
                left = curkey;
                break;
            case 5:
                slowMode = curkey;
                break;
            case 6:
                fire = curkey;
                break;
            case 7:
                escape = curkey;
                break;
            default: break;
        }
        return true;
    }

    public static bool curKeyNull()
    {
        var keys = kstate.GetPressedKeys();
        if (keys.Length > 0)
        {
            return false;
        }
        return true;
    }
}
