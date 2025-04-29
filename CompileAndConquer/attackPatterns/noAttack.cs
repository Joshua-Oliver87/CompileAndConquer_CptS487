using Microsoft.Xna.Framework;
using System;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Entities.BulletSpawners;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

namespace CompileAndConquer.Content.AttackPatterns
{
    /// <summary>
    /// An attack pattern that never fires.
    /// </summary>
    public class noAttack : IAttackPattern
    {
        private float timer = 0f;
    	private bool hasFired = false;
        public bool IsAttackInProgress => false;
        public void Reset()
        {
            timer = 0f;
            hasFired = false;
        }

        public void Update(Enemy enemy, GameTime gameTime)
        {
            // Intentionally empty
        }
    }
}