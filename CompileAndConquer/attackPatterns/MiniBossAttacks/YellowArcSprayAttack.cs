using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

public class ArcAttackPattern : IAttackPattern
{
    private readonly float cooldown;
    private float          timer;
    private bool           inBurst;
    private readonly Func<Vector2, IMovementStrategy> movementFactory;

    private readonly Color color     = Color.Red;
    private readonly int   arcs         = 3;
    private readonly int   bulletsPerArc= 12;
    private readonly float arcSpread    = MathHelper.PiOver4;

    public ArcAttackPattern(Func<Vector2, IMovementStrategy> movementFactory, float cooldown = 1f)
    {
        this.movementFactory = movementFactory;
        this.cooldown        = cooldown;
        Reset();
    }

    public void Reset()
    {
        timer   = 0f;
        inBurst = true;    // start paused in the burst phase
    }

    public bool IsAttackInProgress => inBurst;

    public void Update(Enemy enemy, GameTime gt)
    {
        timer += (float)gt.ElapsedGameTime.TotalSeconds;

        // Wait until our cooldown has elapsed
        if (inBurst && timer >= cooldown)
        {
            Vector2 origin = enemy.Position;
            for (int arc = 0; arc < arcs; arc++)
            {
                float baseAngle = -arcSpread/2 + arc * (arcSpread / (arcs-1));
                for (int i = 0; i < bulletsPerArc; i++)
                {
                    float t     = i/(float)(bulletsPerArc-1);
                    float angle = baseAngle*(1 - t) + (float)Math.PI/2;
                    Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    var movement = movementFactory(dir);
                    EntityHandler.Add(new EnemyBullet(origin, movement, color));
                }
            }

            // mark the burst done so the boss can move again
            inBurst = false;
        }
    }
}
