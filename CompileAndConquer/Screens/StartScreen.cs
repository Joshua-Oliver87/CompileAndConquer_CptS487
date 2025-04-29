using System;
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Screens;

public class StartScreen : Screen
{
    public Action OnExitGame { get; set; }

    private SettingMenuScreen settingMenu;

    private int selectedOption;

    public void Initialize(SpriteFont font)
    {
        this.font = font;
        MenuOptions = ["Start Game","Settings", "Quit"];
    }

    public void SetSettingScreen(SettingMenuScreen settingScreen)
    {
        this.settingMenu = settingScreen;
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
            // start game
            this.screenContext.isVisible = false;
        }
        else if(enter == 1)
        {
            this.screenContext.SetScreen(settingMenu);
        }
        else if(enter == 2)
        {
            OnExitGame?.Invoke();
        }
        else
        {
            // no option was selected.
        }

    }
}