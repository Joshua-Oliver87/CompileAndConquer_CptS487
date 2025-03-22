using Microsoft.Xna.Framework;
using System;

namespace CompileAndConquer.Content
{
    public class LinearMovement : IMovementStrategy
    {
        private float speed;
        private bool movingRight;
        private float minSpeed = 100f;
        private float maxSpeed = 300f;

        private double timeSinceLastChange = 0;
        private double changeInterval = 1.5; // seconds
        private Random rng = new Random();

        public LinearMovement(bool startRight = true, float initialSpeed = 200f)
        {
            this.speed = initialSpeed;
            this.movingRight = startRight;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update position
            float movement = speed * deltaTime * (movingRight ? 1 : -1);
            entity.Position.X += movement;

            // Screen bounds
            float leftBound = 0;
            float rightBound = CompileAndConquer.ScreenSize.X - entity.Size.X;

            if (entity.Position.X <= leftBound)
            {
                entity.Position.X = leftBound;
                movingRight = true;
            }
            else if (entity.Position.X >= rightBound)
            {
                entity.Position.X = rightBound;
                movingRight = false;
            }

            // Randomize speed occasionally
            timeSinceLastChange += deltaTime;
            if (timeSinceLastChange >= changeInterval)
            {
                speed = (float)rng.NextDouble() * (maxSpeed - minSpeed) + minSpeed * 2;
                timeSinceLastChange = 0;
            }
        }
    }
}