using System;
using System.Data;
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
namespace CompileAndConquer.Content;

public abstract class Enemy : Entity
{
    public float Speed { get; protected set; } = 2.0f;    //default speed for enemies

	public AIComponent AIComponent { get; set; }
	
	public bool IsInvulnerable { get; set; } = false;

	public IMovementStrategy movementStrategy;
	
	
	public IExit ExitBehavior { get; set; } 
	public IEntry EntryBehavior { get; set; }


	protected virtual void HandleAttack(GameTime gameTime) { }
	public bool CanMove { get; set; } = true;


	

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
	    //Console.WriteLine($"[Update] {GetHashCode()} Enter={IsEntering} Exit={IsExiting} Pos={Position}");
	    // 1) EXIT has priority
	    if (IsExiting)
	    {
		    ExitBehavior?.Exit(this, gameTime);
		    if (Position.Y < -Size.Y || Position.Y > CompileAndConquer.ScreenSize.Y ||
		        Position.X < -Size.X || Position.X > CompileAndConquer.ScreenSize.X)
		    {
			    EntityHandler.RemoveEntity(this);
			    Console.WriteLine($"[Entity] Removed: {GetType().Name} at {Position}");
			    isAlive = false;
		    }
		    return;
	    }

	    // 2) ENTRY if we're still entering
	    if (IsEntering)
	    {
		    EntryBehavior?.Enter(this, gameTime);
		    if (EntryBehavior?.HasEntered(this) == true)
			    IsEntering = false;
		    return;
	    }
	    
	    // 3) NORMAL
	    if (!IsEntering && !IsExiting && isAlive)
	    {
		    HandleAttack(gameTime);
		    if (CanMove)
			    movementStrategy?.Move(this, gameTime);
	    }
    }



    
    public override void OnCollide(Entity collision)
    {
	    if (IsInvulnerable)
	    {
		    return;
	    }
	    if (collision is PlayerBullet)
	    {
		    this.health -= collision.Damage;

		    CompileAndConquer.score += 1;
		    
		    if (this.health <= 0)
			    this.isAlive = false;
	    }
    }

    
    public abstract void Attack();
}