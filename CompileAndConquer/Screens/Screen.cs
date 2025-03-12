//should have the base screen stats like the hearts and score
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace CompileAndConquer.Screens;

public abstract class Screen
{
    private bool show = false; // is it visible on screen?
    
    public virtual void ShowScreen()
    {}

    public virtual void HideScren()
    {}
}