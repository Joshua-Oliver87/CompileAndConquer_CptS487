using Microsoft.Xna.Framework;
using System;

/*
 * Movement for Final Boss
 * Moves left to right bouncing off screen bounds
 * While drifitng horizontally, it bobs up and down via vertical sine wave
 */

namespace CompileAndConquer.Content
{
    public class FinalBossMovement : IMovementStrategy
    {
        private float horizontalSpeed = 50f;
        private float verticalAmplitude = 25f;
        private float verticalFrequency = 2f;

        private float elapsedTime = 0f;
        private bool movingRight = true;

        private float leftBound = 100f;
        private float rightBound = 800f;

        public FinalBossMovement(float horizontalSpeed = 50f, float verticalAmplitude = 25f, float verticalFrequency = 2f)
        {
            this.horizontalSpeed = horizontalSpeed;
            this.verticalAmplitude = verticalAmplitude;
            this.verticalFrequency = verticalFrequency;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += delta;

            // Horizontal drift
            if (movingRight)
            {
                entity.Position.X += horizontalSpeed * delta;
                if (entity.Position.X >= rightBound)
                {
                    entity.Position.X = rightBound;
                    movingRight = false;
                }
            }
            else
            {
                entity.Position.X -= horizontalSpeed * delta;
                if (entity.Position.X <= leftBound)
                {
                    entity.Position.X = leftBound;
                    movingRight = true;
                }
            }

            // Vertical bobbing (sine wave)
            float yOffset = (float)Math.Sin(elapsedTime * verticalFrequency) * verticalAmplitude;
            entity.Position.Y = 150 + yOffset;
        }
    }
}