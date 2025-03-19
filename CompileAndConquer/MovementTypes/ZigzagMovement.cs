using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public class ZigZagMovement : IMovementStrategy
    {
        private bool movingRight = true;
        private float leftBound;
        private float rightBound;
        private float speed;

        public ZigZagMovement(float leftBound, float rightBound, float speed)
        {
            this.leftBound = leftBound;
            this.rightBound = rightBound;
            this.speed = speed;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            if (movingRight)
            {
                entity.Position.X += speed;
                if (entity.Position.X >= rightBound)
                    movingRight = false;
            }
            else
            {
                entity.Position.X -= speed;
                if (entity.Position.X <= leftBound)
                    movingRight = true;
            }
        }
    }
}