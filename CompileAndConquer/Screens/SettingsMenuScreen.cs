using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace CompileAndConquer.Content;

public class SettingMenuScreen : Screen
{
    private int selectedOption;
    private PauseMenuScreen pauseMenu;
    
    // private int playerHealth = 5; // example value, set this dynamically
    // private int playerScore = 1000; // example value, set this dynamically

    public void Initialize(SpriteFont font)
    {
        this.font = font;
        this.MenuOptions = new string[] { "Change Keybinds","Return to Menu" };

    }

    public void SetPauseScreen(PauseMenuScreen pauseMenuScreen)
    {
        this.pauseMenu = pauseMenuScreen;
    }


    public override void Draw(SpriteBatch spriteBatch)
    {
        // // Display base stats (hearts, score) at the top
        // Vector2 statsPosition = new Vector2(10, 10); // Adjust position as needed
        // spriteBatch.DrawString(font, $"Health: {playerHealth}", statsPosition, Color.White);
        // spriteBatch.DrawString(font, $"Score: {playerScore}", new Vector2(statsPosition.X, statsPosition.Y + 30), Color.White);

        Vector2 menuPosition = new Vector2(CompileAndConquer.ScreenSize.X / 2 - 50, CompileAndConquer.ScreenSize.Y / 2 - 50);

        for (int i = 0; i < MenuOptions.Length; i++)
        {
            Color color;
            if (i == selectedOption)
            {
                color = Color.Peru;
            }
            else
            {
                color = Color.White;
            }
            spriteBatch.DrawString(font, MenuOptions[i], menuPosition, color);
            menuPosition.Y += 40; 
        }
    }

    public override void Update()
    {
        int enter = InputHandler.HandleScreenInput(this, ref selectedOption);
        if (enter == 0)
        {
            //change keybinds logic
        }
        else if(enter == 1)
        {
            if(pauseMenu !=null)
            {
            this.screenContext.SetScreen(pauseMenu);}
            else{
                Console.WriteLine("ERROR: in setting screen, pause is null.");
            }

        }
        else
        {
            // no option was selected.
        }

    }
}