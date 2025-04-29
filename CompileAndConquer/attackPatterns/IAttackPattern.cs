using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.AttackPatterns
{
    public interface IAttackPattern
    {
        void Update(Enemy enemy, GameTime gameTime);
		
		void Reset();
        
        bool IsAttackInProgress { get; }
    }
}