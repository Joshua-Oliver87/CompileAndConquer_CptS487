using System;
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Screens;

public class EndScreen : Screen
{
    private static EndScreen instance;
    public  bool gameEnd { get; set; } = false;
    public Action OnExitGame { get; set; }

    private int selectedOption;

    public void Initialize(SpriteFont font)
    {
        this.font = font;
        MenuOptions = new[] { "Return to Main Menu", "Quit" };
    }

    public static EndScreen Instance
    {
        get{
            if (instance == null)
            {
                instance = new EndScreen();
            }
            return instance;
        }
        
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
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
            gameEnd = false;
            // switch to main menu when created
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