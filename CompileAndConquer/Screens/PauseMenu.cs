using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Screens;

public class PauseMenuScreen : Screen
{
    private static PauseMenuScreen instance;
    public  bool isPaused { get; set; } = false;
    private string[] menuOptions = { "Resume", "Quit" };
    public Action OnExitGame { get; set; }

    private int selectedOption;

    public void Initialize(SpriteFont font)
    {
        this.font = font;
    }

    public static PauseMenuScreen Instance
    {
        get{
            if (instance == null)
            {
                instance = new PauseMenuScreen();
            }
            return instance;
        }
        
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 menuPosition = new Vector2(CompileAndConquer.ScreenSize.X / 2 - 50, CompileAndConquer.ScreenSize.Y / 2 - 50);

        for (int i = 0; i < menuOptions.Length; i++)
        {
            Color color;
            if (i == selectedOption)
            {
                color = Color.Yellow;
            }
            else
            {
                color = Color.White;
            }
            spriteBatch.DrawString(font, menuOptions[i], menuPosition, color);
            menuPosition.Y += 40; 
        }
    }

    public override void Update()
    {
        int enter = InputHandler.HandlePauseMenuInput(ref selectedOption);
        if (enter == 0)
        {
            isPaused = false;

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