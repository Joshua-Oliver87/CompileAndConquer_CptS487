using Microsoft.Xna.Framework;
using CompileAndConquer.Content.Interfaces;

namespace CompileAndConquer.Content
{
    /// <summary>
    /// A “do‐nothing” movement strategy.
    /// </summary>
    public class StationaryMovement : IMovementStrategy
    {
        public void Move(Entity entity, GameTime gameTime)
        {
            // empty
        }
    }
}