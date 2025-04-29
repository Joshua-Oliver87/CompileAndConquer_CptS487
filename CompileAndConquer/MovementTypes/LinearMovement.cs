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

        public LinearMovement(bool startRight, float initialSpeed = 150f)
        {
            this.movingRight = startRight;
            this.speed = initialSpeed;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
			//Console.WriteLine($"[Move]  {entity.GetHashCode()} DIR={(movingRight?">": "<")} POS={entity.Position}");
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
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

            // random speed occasionally
            timeSinceLastChange += deltaTime;
            if (timeSinceLastChange >= changeInterval)
            {
                speed = (float)rng.NextDouble() * (maxSpeed - minSpeed) + minSpeed;
                timeSinceLastChange = 0;
            }
        }
    }
}