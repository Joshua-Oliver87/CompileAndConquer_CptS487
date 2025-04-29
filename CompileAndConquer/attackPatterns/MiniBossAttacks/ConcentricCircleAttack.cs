using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

public class ConcentricCircleAttack : IAttackPattern
{
    private readonly int rings;
    private readonly int perRing;
    private readonly float burstDuration;
    private readonly Func<Vector2, float, IMovementStrategy> bulletFactory;

    private float timer;
    private bool inBurst;
    private bool hasSpawned;

    public bool IsAttackInProgress => inBurst;

    public ConcentricCircleAttack(
        Func<Vector2, float, IMovementStrategy>? bulletFactory = null,
        int rings = 2,
        int perRing = 16,
        float burstDuration = 1.0f
    )
    {
        this.rings = rings;
        this.perRing = perRing;
        this.burstDuration = burstDuration;
        this.bulletFactory = bulletFactory ?? DefaultBulletFactory;
        Reset();
    }

    public void Reset()
    {
        timer = 0f;
        inBurst = true;
        hasSpawned = false;
    }

    public void Update(Enemy enemy, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (inBurst && !hasSpawned)
        {
            hasSpawned = true;
            Vector2 origin = enemy.Position;

            for (int r = 1; r <= rings; r++)
            {
                for (int i = 0; i < perRing; i++)
                {
                    float ang = i * MathHelper.TwoPi / perRing;
                    Vector2 dir = new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang));
                    Vector2 spawn = origin + dir * (r * 30f);

                    float speed = 150f + r * 20f;
                    var move = bulletFactory(dir, speed);

                    EntityHandler.Add(new EnemyBullet(spawn, move, Color.Blue));
                }
            }
        }

        if (inBurst)
        {
            timer += dt;
            if (timer >= burstDuration)
                inBurst = false;
        }
    }

    private IMovementStrategy DefaultBulletFactory(Vector2 direction, float speed)
    {
        return new StraightBullet(direction, speed);
    }
}
