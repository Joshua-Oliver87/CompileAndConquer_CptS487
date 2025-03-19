using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public class VerticalBounceMovement : IMovementStrategy
    {
        private bool movingDown = true;
        private float topBound;
        private float bottomBound;
        private float speed;

        public VerticalBounceMovement(float topBound, float bottomBound, float speed)
        {
            this.topBound = topBound;
            this.bottomBound = bottomBound;
            this.speed = speed;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            if (movingDown)
            {
                entity.Position.Y += speed;
                if (entity.Position.Y >= bottomBound)
                    movingDown = false;
            }
            else
            {
                entity.Position.Y -= speed;
                if (entity.Position.Y <= topBound)
                    movingDown = true;
            }
        }
    }
}