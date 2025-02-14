using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

// ranged enemy
public class TypeB : Enemy
{
    private bool movingRight = false;
	private float leftBound = 10;
	private float rightBound = 1000;
    public TypeB(Texture2D texture, Vector2 position) : base(texture, position, 2.5f, 0.75f)
    {
        this.health = 120; // default
    }

    // zigzag pattern
    public override void Update()
    {
		this.Position.Y = 50;
        if (movingRight)
        { 
            this.Position.X += Speed;
			if (Position.X >= rightBound)
			{
				movingRight = false;
        	}
		}
        else
        {
            this.Position.X -= Speed;
			if (Position.X <= leftBound)
		 	{
        		movingRight = true;
			}
    	}
	}

    public override void Attack()
    {
      //  Game1.SpawnProjectile(this.Position, Player.Instance.Position);
    }

	public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Position, null, color, Orientation, Size / 2f, ScaleFactor, 0, 0);
    }
}