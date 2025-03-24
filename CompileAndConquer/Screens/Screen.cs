//should have the base screen stats like the hearts and score
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Diagnostics;

namespace CompileAndConquer.Content;

public abstract class Screen
{
    protected SpriteFont font;
    // private bool show = false; // is it visible on screen?

    public string[] MenuOptions;
    
    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Update();
}