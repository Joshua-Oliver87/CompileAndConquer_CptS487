using Microsoft.Xna.Framework;
using System;

namespace CompileAndConquer.Content
{
    public class CurvedMovement : IMovementStrategy
    {
        private float amplitude;         // Height of bounce
        private float frequency;         // Controls wave tightness
        private float baseSpeed;         // Horizontal base speed
        private float dartSpeedY;        // Speed for darting up
        private float elapsedTime = 0f;

        private float startY = 50f;
        private bool movingRight = false; // Start moving left

        public CurvedMovement(float baseSpeed = 250f, float amplitude = 80f, float frequency = 6f, float dartSpeedY = 60f)
        {
            this.baseSpeed = baseSpeed;
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.dartSpeedY = dartSpeedY;
        }

        private float verticalVelocity = 0f;

        public void Move(Entity entity, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += deltaTime;
            
            float horizontalDirection = movingRight ? 1 : -1;
            entity.Position.X += horizontalDirection * baseSpeed * deltaTime;

            // Flip direction if hitting boundaries
            if (entity.Position.X <= 0)
            {
                entity.Position.X = 0;
                movingRight = true;
            }
            else if (entity.Position.X + entity.Size.X >= CompileAndConquer.ScreenSize.X)
            {
                entity.Position.X = CompileAndConquer.ScreenSize.X - entity.Size.X;
                movingRight = false;
            }
            
            float sinValue = (float)Math.Sin(elapsedTime * frequency);

            if (sinValue > 0)
            {
                // downward wave
                entity.Position.Y = startY + sinValue * amplitude;
                verticalVelocity = 0f; // reset upward velocity
            }
            else
            {
                // ease upward using velocity
                verticalVelocity -= dartSpeedY * deltaTime;
                entity.Position.Y += verticalVelocity * deltaTime;
                if (entity.Position.Y < 0) entity.Position.Y = 0;
            }

            // despawn if off bottom
            if (entity.Position.Y > CompileAndConquer.ScreenSize.Y)
                EntityHandler.RemoveEntity(entity);
        }
    }
}
