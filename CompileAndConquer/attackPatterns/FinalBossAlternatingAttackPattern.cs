using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

public class AlternatingAttackPattern : IAttackPattern
{
    private IAttackPattern[] patterns;
    private int current = 0;

    private float pauseBetween;
    private float pauseTimer = 0f;
    private bool inPause = false;
    private bool hasFired = false;

	public bool IsAttackInProgress => !inPause || !hasFired;


    public AlternatingAttackPattern(float pauseBetweenAttacks, params IAttackPattern[] patterns)
    {
        this.patterns = patterns;
        this.pauseBetween = pauseBetweenAttacks;
		Reset();
    }

    public void Reset()
    {
        current = 0;
        pauseTimer = 0f;
        inPause = false;
        hasFired = false;
        foreach (var p in patterns)
        	p.Reset();
    }


    public void Update(Enemy e, GameTime gt)
    {
		Console.WriteLine($"AlternatingAttackPattern: current={current}, inPause={inPause}, attackInProgress={patterns[current].IsAttackInProgress}");
        if (inPause)
        {
            pauseTimer += (float)gt.ElapsedGameTime.TotalSeconds;
            if (pauseTimer >= pauseBetween)
            {
                // End pause, switch to next pattern
                pauseTimer = 0f;
                inPause = false;
				hasFired = false;
				current = (current + 1) % patterns.Length;
				patterns[current].Reset();
				Console.WriteLine($"AlternatingAttackPattern: switched to {current}");
            }
            return;
        }

        patterns[current].Update(e, gt);

        // When the current attack pattern is done, start pause (before switching)
        if (!patterns[current].IsAttackInProgress)
        {
			hasFired = true;
            inPause = true;
			Console.WriteLine($"AlternatingAttackPattern: pattern {current} finished, starting pause");
        }
    }
}
