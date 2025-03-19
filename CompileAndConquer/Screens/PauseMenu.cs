using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Screens;

public class PauseMenuScreen : Screen
{
    // private bool isPaused = false;
    private string[] menuOptions = { "Resume", "Quit" };
    private int selectedOption;

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
            spriteBatch.DrawString(menuFont, menuOptions[i], menuPosition, color);
            menuPosition.Y += 40; 
        }
    }

    public override void Update()
    {
        InputHandler.HandlePauseMenuInput();
    }
}