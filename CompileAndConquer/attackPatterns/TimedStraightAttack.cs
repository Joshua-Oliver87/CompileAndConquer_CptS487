// fires a straight bullet every fireInterval seconds
using Microsoft.Xna.Framework;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
public class TimedStraightAttack : IAttackPattern
{
    private readonly float fireInterval;
    private readonly float bulletSpeed;
    private float timer;

    public TimedStraightAttack(float fireInterval = 0.5f, float bulletSpeed = 200f)
    {
        this.fireInterval = fireInterval;
        this.bulletSpeed  = bulletSpeed;
    }

    public void Update(Enemy enemy, GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer < fireInterval || enemy.BulletMovementStrategy == null)
            return;

        // spawn one bullet straight down:
        EntityHandler.Add(new EnemyBullet(
            enemy.Position,
            new StraightBullet(new Vector2(0,1), bulletSpeed)
        ));

        timer = 0;
    }
}