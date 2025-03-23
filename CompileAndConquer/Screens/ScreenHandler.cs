// should handle the differen screens, will handle showing the differnt screens

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Screens;

public class ScreenHandler
{
    public static PauseMenuScreen menuScreen;
    public static void Update()
    {
        if (InputHandler.PauseGame())
        {
            // menuScreen.ShowScreen();

        }
    }
}