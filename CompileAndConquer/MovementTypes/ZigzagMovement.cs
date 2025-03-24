using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content
{
    public class ZigZagMovement : IMovementStrategy
    {
        private bool movingRight = true;
        private bool movingDown = true;

        private float leftBound;
        private float rightBound;
        private float topBound;
        private float bottomBound;

        private float horizontalSpeed;
        private float verticalSpeed;

        public ZigZagMovement(
            float leftBound, float rightBound,
            float topBound, float bottomBound,
            float horizontalSpeed = 150f, float verticalSpeed = 75f)
        {
            this.leftBound = leftBound;
            this.rightBound = rightBound;
            this.topBound = topBound;
            this.bottomBound = bottomBound;
            this.horizontalSpeed = horizontalSpeed;
            this.verticalSpeed = verticalSpeed;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Horizontal bounce
            if (movingRight)
            {
                entity.Position.X += horizontalSpeed * deltaTime;
                if (entity.Position.X >= rightBound)
                {
                    entity.Position.X = rightBound;
                    movingRight = false;
                }
            }
            else
            {
                entity.Position.X -= horizontalSpeed * deltaTime;
                if (entity.Position.X <= leftBound)
                {
                    entity.Position.X = leftBound;
                    movingRight = true;
                }
            }

            // Vertical bounce
            if (movingDown)
            {
                entity.Position.Y += verticalSpeed * deltaTime;
                if (entity.Position.Y >= bottomBound)
                {
                    entity.Position.Y = bottomBound;
                    movingDown = false;
                }
            }
            else
            {
                entity.Position.Y -= verticalSpeed * deltaTime;
                if (entity.Position.Y <= topBound)
                {
                    entity.Position.Y = topBound;
                    movingDown = true;
                }
            }
        }
    }
}
