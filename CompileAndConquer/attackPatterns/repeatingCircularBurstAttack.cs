using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.Entities.BulletSpawners;

namespace CompileAndConquer.Content.AttackPatterns
{
    public class RepeatingSpiralAttack : IAttackPattern
	{
    	private float burstDuration = 1.6f;
    	private float pauseDuration = 1.4f;
    	private float cycleTimer = 0f;
    	private bool inBurst = false;

    	private bool spawnersCreated = false;

    	public void Update(Enemy enemy, GameTime gameTime)
    	{
        	float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        	cycleTimer += delta;

        	// Disable movement while attacking
        	enemy.CanMove = !inBurst;

        	if (!inBurst)
        	{
            	if (cycleTimer >= pauseDuration)
            	{
                	inBurst = true;
                	cycleTimer = 0f;
                	spawnersCreated = false;
            	}
            	return;
        	}

        	if (inBurst && cycleTimer >= burstDuration)
        	{
            	inBurst = false;
            	cycleTimer = 0f;
            	return;
        	}

        	if (inBurst && !spawnersCreated)
        	{
            	Vector2 basePos = new Vector2(
                	enemy.Position.X,
                	enemy.Position.Y + enemy.Size.Y / 2f
            	);

            	// Offset left and right to form dual spirals
            	float offsetX = 20f;

            	var leftFactory = new SpiralBulletSpawnerFactory(basePos + new Vector2(-offsetX, 0));
				EntityHandler.Add(leftFactory.Create());

				var rightFactory = new SpiralBulletSpawnerFactory(basePos + new Vector2(offsetX, 0));
				EntityHandler.Add(rightFactory.Create());


            	spawnersCreated = true;
        	}
    	}
	}
}
