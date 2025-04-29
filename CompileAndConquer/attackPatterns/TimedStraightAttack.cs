// fires a straight bullet every fireInterval seconds
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
    public bool IsAttackInProgress => false;
	private Color color = Color.Yellow;
    private bool hasFired = false;

    public TimedStraightAttack(Func<Vector2, IMovementStrategy> bulletFactory, float fireInterval = 0.2f)
    {
		this._bulletFactory = bulletFactory;
        this.fireInterval = fireInterval;
    }

    public void Reset()
    {
        timer = 0f;
        hasFired = false;
    }


    public void Update(Enemy enemy, GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer < fireInterval)
            return;

        var dir = new Vector2(0,1);

        var move = _bulletFactory(dir);
        EntityHandler.Add(new EnemyBullet(enemy.Position, move, color));

        timer = 0;
    }
}