using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.Interfaces
{
    public interface IAttackPattern
    {
        void Update(Enemy enemy, GameTime gameTime);
		
		void Reset();
        
        bool IsAttackInProgress { get; }
    }
}