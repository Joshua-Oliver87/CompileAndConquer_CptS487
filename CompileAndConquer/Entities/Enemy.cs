using System;
using System.Data;
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
namespace CompileAndConquer.Content;

public abstract class Enemy : Entity
{
    public float Speed { get; protected set; } = 2.0f;    //default speed for enemies

	public AIComponent AIComponent { get; set; }

	public IMovementStrategy movementStrategy;
	
	public IMovementStrategy BulletMovementStrategy { get; set; } = null;
	
	public IExit ExitBehavior { get; set; } 
	

    public Enemy(Texture2D texture, Vector2 position, float speed, float scalePercentage, IMovementStrategy movementStrategy)
    {
        this.texture = texture;
        this.Position = position;
        this.Speed = speed;
        this.health = 100; // default
		this.movementStrategy = movementStrategy;
        
        Collidable = true;
        
        // ScaleFactor for enemies relative to player size
        this.Scale = scalePercentage * (Player.Instance.Size.X / texture.Width);
        
        health = 1;
    }
    
    protected bool CanAttack()
    {
	    return !IsExiting && !IsEntering;
    }

    public override void Update(GameTime gameTime)
    {
	    if (IsEntering)
		    return;

	    if (IsExiting && ExitBehavior != null)
	    {
		    ExitBehavior.Exit(this, gameTime);

		    // Remove the entity if it’s off-screen
		    if (Position.Y < -Size.Y || Position.Y > CompileAndConquer.ScreenSize.Y)
		    {
			    EntityHandler.RemoveEntity(this);
			    isAlive = false;
		    }

		    return;
	    }

	    movementStrategy?.Move(this, gameTime);
    }

    
    public abstract void Attack();
}