// should handle the differen screens, will handle showing the differnt screens

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Content;

public class ScreenContext
{
    private bool initalized = false; // ensure singleton pattern
    private Screen curScreen;
    private SpriteFont font;
    private static ScreenContext instance;
    public static ScreenContext Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScreenContext();
            }
            return instance;
        }
    }

    public bool isVisible
    {
        get;
        set;
    }

    public void Initialize(SpriteFont font, Screen screen)
    {
        if (initalized == true) //we already have screencontext
        {
            Console.WriteLine("Already have initilized screencontext");
            return;
        }
        else{
            initalized =true;
            isVisible = true;
            this.font = font;
            this.curScreen = screen;
        }
    }

    public void SetScreen(Screen screen)
    {
        isVisible = true;
        curScreen = screen;
    }

    /// <summary>
    /// call the update funciton for whatever screen is the current one.
    /// </summary>
    public void Update()
    {
        //future implemenation
        curScreen?.Update();
    }

    /// <summary>
    /// call the draw funciton for whatever screen is the current one.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch)
    {
        curScreen?.Draw(spriteBatch);
    }
}