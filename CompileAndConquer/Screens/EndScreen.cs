using System;
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Screens;

public class EndScreen : Screen
{
    public bool win {get;set;} = false;
    public Action OnExitGame { get; set; }

    private StartScreen startScreen;

    private int selectedOption;

    public void Initialize(SpriteFont font)
    {
        this.font = font;
        MenuOptions = ["Return to Main Menu", "Quit"];
    }

    public void SetStartScreen(StartScreen startScreen)
    {
        this.startScreen = startScreen;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 menuPosition = new Vector2(CompileAndConquer.ScreenSize.X / 2 - 50, CompileAndConquer.ScreenSize.Y / 2 - 50);
        if (this.win == true)
        {
            spriteBatch.DrawString(font, "You Win!", menuPosition, Color.White);

        }
        else
        {
            spriteBatch.DrawString(font, "Game Over", menuPosition, Color.White);

        }
        
        menuPosition.Y += 40; 

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
            // switch to start when created
            this.screenContext.SetScreen(startScreen);

        }
        else if(enter == 1)
        {
            OnExitGame?.Invoke();
        }
        else
        {
            // no option was selected.
        }

    }
}