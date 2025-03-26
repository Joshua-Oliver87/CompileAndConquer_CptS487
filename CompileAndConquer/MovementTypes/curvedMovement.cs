using Microsoft.Xna.Framework;
using System;

namespace CompileAndConquer.Content
{
    public class CurvedMovement : IMovementStrategy
    {
        private float horizontalAmplitude;  // Width of the horizontal wave
        private float verticalAmplitude;    // Height of the vertical wave
        private float horizontalFrequency;  // Controls how tight the horizontal wave is
        private float verticalFrequency;    // Controls how fast the vertical oscillation is

        private float elapsedTime = 0f;
        private Vector2 centerPoint;

        public CurvedMovement(
            float horizontalAmplitude = 200f,
            float verticalAmplitude = 80f,
            float horizontalFrequency = 1.0f,
            float verticalFrequency = 2.0f)
        {
            this.horizontalAmplitude = horizontalAmplitude;
            this.verticalAmplitude = verticalAmplitude;
            this.horizontalFrequency = horizontalFrequency;
            this.verticalFrequency = verticalFrequency;
        }

        public void Move(Entity entity, GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Store starting position as the center once
            if (centerPoint == Vector2.Zero)
            {
                centerPoint = entity.Position;
            }

            float x = centerPoint.X + (float)Math.Sin(elapsedTime * horizontalFrequency) * horizontalAmplitude;
            float y = centerPoint.Y + (float)Math.Sin(elapsedTime * verticalFrequency) * verticalAmplitude;

            entity.Position = new Vector2(x, y);
        }
    }
}