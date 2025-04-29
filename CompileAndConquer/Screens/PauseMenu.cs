using CompileAndConquer.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CompileAndConquer.Content;

public class PauseMenuScreen : Screen
{
    //private static PauseMenuScreen instance;
    public Action OnExitGame { get; set; }
    private SettingMenuScreen settingMenu;
    private EndScreen endScreen;

    private int selectedOption;
    
    // private int playerHealth = 5; // example value, set this dynamically
    // private int playerScore = 1000; // example value, set this dynamically

    public void Initialize(SpriteFont font)
    {
        this.font = font;
        this.MenuOptions = new string[] { "Resume","Settings", "Quit" };
    }

    public void SetEndScreen(EndScreen endScreen)
    {
        this.endScreen = endScreen;
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
            this.screenContext.isVisible = false;

        }
        else if(enter ==1)
        {
            //settings
            if(settingMenu !=null)
            {
                this.screenContext.SetScreen(settingMenu);
            }
            else
            {
                Console.WriteLine("ERROR: in pause screen, setting is null.");
            }
        }
        else if(enter == 2)
        {
            //dirrectly exit the game
            OnExitGame?.Invoke();

            //alternativly, take to exit game screen?
            this.screenContext.SetScreen(endScreen);
        }
        else
        {
            // no option was selected.
        }

    }
}