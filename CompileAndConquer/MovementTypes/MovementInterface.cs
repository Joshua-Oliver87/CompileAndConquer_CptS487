using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public interface IMovementStrategy
    {
        void Move(Entity entity, GameTime gameTime);
    }
}