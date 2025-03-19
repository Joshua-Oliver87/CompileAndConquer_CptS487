using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace CompileAndConquer;

public class InputHandler
{
    private static KeyboardState kstate;

    public static void Update(GameTime gameTime)
    {
        kstate = Keyboard.GetState();
    }

    public static Microsoft.Xna.Framework.Vector2 GetMovement()
	{
    	Microsoft.Xna.Framework.Vector2 movement = new Microsoft.Xna.Framework.Vector2();
    
    	if (kstate.IsKeyDown(Keys.Up)) 
        	movement.Y -= 1;
    	if (kstate.IsKeyDown(Keys.Down)) 
        	movement.Y += 1;
    	if (kstate.IsKeyDown(Keys.Left)) 
        	movement.X -= 1;
    	if (kstate.IsKeyDown(Keys.Right)) 
        	movement.X += 1;
    
    	if (movement.Length() > 0)
    	{
        	movement = Microsoft.Xna.Framework.Vector2.Normalize(movement);
    	}
    	return movement;
	}
    
    public static bool GetFire() { return kstate.IsKeyDown(Keys.Space); }
    
    public static bool GetSlowMode() { return kstate.IsKeyDown(Keys.LeftControl); }
}