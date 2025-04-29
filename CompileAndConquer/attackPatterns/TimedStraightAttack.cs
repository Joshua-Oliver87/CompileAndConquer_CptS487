using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
public class TimedStraightAttack : IAttackPattern
{
    private readonly float fireInterval;
    private readonly Func<Vector2, IMovementStrategy> _bulletFactory;
    private float timer;
    private bool firingCycleActive = false;
    private float cycleDuration = 2f; 
    private float cycleTimer = 0f;

    public bool IsAttackInProgress => firingCycleActive;

    public TimedStraightAttack(Func<Vector2, IMovementStrategy> bulletFactory, float fireInterval = 0.2f)
    {
        _bulletFactory = bulletFactory;
        this.fireInterval = fireInterval;
    }

    public void Reset()
    {
        timer = 0f;
        cycleTimer = 0f;
        firingCycleActive = false;
    }

    public void Update(Enemy enemy, GameTime gameTime)
    {
        if (!firingCycleActive)
        {
            firingCycleActive = true;
            cycleTimer = 0f;
        }

        cycleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (cycleTimer >= cycleDuration)
        {
            firingCycleActive = false;
            return; // stop firing after cycle ends
        }

        if (timer >= fireInterval)
        {
            var dir = new Vector2(0, 1);
            var move = _bulletFactory(dir);
            EntityHandler.Add(new EnemyBullet(enemy.Position, move, Color.Yellow));
            timer = 0;
        }
    }
}
