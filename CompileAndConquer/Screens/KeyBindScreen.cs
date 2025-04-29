using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace CompileAndConquer.Content;

public class KeyBindScreen : Screen
{
    private int selectedOption;
    private bool enterPressed = false;
    private bool needUserInput = false;
    private SettingMenuScreen settingMenu;

    // private int playerHealth = 5; // example value, set this dynamically
    // private int playerScore = 1000; // example value, set this dynamically

    public void Initialize(SpriteFont font)
    {
        this.font = font;
        this.MenuOptions = new string[] { "Up", "Down", "Right", "Left", "Slow Mode", "Fire", "Pause", "Return to Settings" };

    }

    public void SetSettingScreen(SettingMenuScreen settingScreen)
    {
        this.settingMenu = settingScreen;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // // Display base stats (hearts, score) at the top
        // Vector2 statsPosition = new Vector2(10, 10); // Adjust position as needed
        // spriteBatch.DrawString(font, $"Health: {playerHealth}", statsPosition, Color.White);
        // spriteBatch.DrawString(font, $"Score: {playerScore}", new Vector2(statsPosition.X, statsPosition.Y + 30), Color.White);

        Vector2 menuPosition = new Vector2(CompileAndConquer.ScreenSize.X / 2 - 150, CompileAndConquer.ScreenSize.Y / 2 - 100);

        for (int i = 0; i < MenuOptions.Length; i++)
        {
            Color color;
            string curkey = MenuOptions[i];
            if (i == selectedOption)
            {
                color = Color.Peru;
            }
            else
            {
                color = Color.White;
            }

            if (i < MenuOptions.Length - 1)
            {
                curkey += " - " + getCurrentKey(i);
            }

            spriteBatch.DrawString(font, curkey, menuPosition, color);
            menuPosition.Y += 40;
        }

        if (enterPressed)
        {
            spriteBatch.DrawString(font, "Please press new key...", new Vector2(CompileAndConquer.ScreenSize.X / 2 - 50, 100), Color.Yellow);
        }
    }

    public override void Update()
    {
        if (enterPressed)
        {
            if (needUserInput)
            {
                if (InputHandler.curKeyNull())
                {
                    needUserInput = false;
                }
                return;

            }

            if (!InputHandler.curKeyNull())
            {
                InputHandler.setCurrentKey(selectedOption);
                enterPressed = false;
            }
            return;
        }

        int enter = InputHandler.HandleScreenInput(this, ref selectedOption);
        if (enter == -1)
        {
            return;
        }
        if (MenuOptions[enter] == "Return to Settings")
        {

            if (settingMenu != null)
            {
                this.screenContext.SetScreen(settingMenu);
            }
            else
            {
                Console.WriteLine("ERROR: in setting screen, pause is null.");
            }

        }
        else
        {
            // no option was selected.
            enterPressed = true;
            needUserInput = true;
        }

    }

    private string getCurrentKey(int i)
    {
        switch (i + 1)
        {
            //"Up","Down", "Right","Left","Slow Mode","Fire","Pause",
            case 1: return InputHandler.up.ToString();
            case 2: return InputHandler.down.ToString();
            case 3: return InputHandler.right.ToString();
            case 4: return InputHandler.left.ToString();
            case 5: return InputHandler.slowMode.ToString();
            case 6: return InputHandler.fire.ToString();
            case 7: return InputHandler.escape.ToString();
            default: return "";
        }
    }
}