using System;
using System.Data;
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public abstract class Enemy : Entity
{
    public float Speed { get; protected set; } = 2.0f;    //default speed for enemies

	public AIComponent AIComponent { get; set; }

	public IMovementStrategy movementStrategy;

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

    public override void Update(GameTime gameTime)
	{
		movementStrategy?.Move(this, gameTime); //apply movement
	}
    
    public abstract void Attack();
}