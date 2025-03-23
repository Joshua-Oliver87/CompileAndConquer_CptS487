using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Screens;

public class SideBarScreen : Screen
{
    private static SideBarScreen instance;
    private Rectangle sideBarRectangle;

    public void Initialize(SpriteFont font)
    {
        this.font = font;
        // this.sideBarRectangle = new Rectangle()
    }

    // public static PauseMenuScreen Instance
    // {
    //     get{
    //         if (instance == null)
    //         {
    //             instance = new PauseMenuScreen();
    //         }
    //         return instance;
    //     }
        
    // }


}