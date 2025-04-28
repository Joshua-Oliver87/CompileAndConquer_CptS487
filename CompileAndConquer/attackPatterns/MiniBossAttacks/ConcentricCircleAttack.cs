using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

public class ConcentricCircleAttack : IAttackPattern
{
    private readonly int   rings         = 2;
    private readonly int   perRing       = 16;
    private readonly float burstDuration = 1.0f;
    private float          timer;
    private bool           inBurst;

    public void Reset()
    {
        timer   = 0f;
        inBurst = true;
    }

    public bool IsAttackInProgress => inBurst;

    public void Update(Enemy enemy, GameTime gt)
    {
        float dt = (float)gt.ElapsedGameTime.TotalSeconds;

        // Spawn once at start of burst
        if (inBurst && timer == 0f)
        {
            Vector2 origin = enemy.Position;
            for (int r = 1; r <= rings; r++)
                for (int i = 0; i < perRing; i++)
                {
                    float ang = i * MathHelper.TwoPi / perRing;
                    Vector2 spawn = origin + new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang)) * (r * 30f);
                    EntityHandler.Add(new EnemyBullet(
                        spawn,
                        new StraightBullet(new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang)), 150f + r * 20f),
                        Color.Blue
                    ));
                }
        }

        // Advance timer, end burst when done
        if (inBurst)
        {
            timer += dt;
            if (timer >= burstDuration)
                inBurst = false;
        }
    }
}
