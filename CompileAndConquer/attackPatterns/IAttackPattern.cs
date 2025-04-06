using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.Interfaces
{
    public interface IAttackPattern
    {
        void Update(Enemy enemy, GameTime gameTime);
    }
}