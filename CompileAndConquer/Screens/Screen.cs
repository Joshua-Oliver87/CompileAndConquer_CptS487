//should have the base screen stats like the hearts and score
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Diagnostics;

namespace CompileAndConquer.Screens;

public abstract class Screen
{
    protected SpriteFont menuFont;
    private bool show = false; // is it visible on screen?
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {}

    public virtual void Update()
    {}
}