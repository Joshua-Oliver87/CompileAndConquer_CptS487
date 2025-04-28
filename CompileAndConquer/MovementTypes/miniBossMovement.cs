using Microsoft.Xna.Framework;
using System;

namespace CompileAndConquer.Content
{
    public class miniBossMovement : IMovementStrategy
    {
        // horizontal speeds (units/sec)
        private readonly float patrolSpeed = 50f;
        private readonly float dashSpeed = 250f;

        // bobbing
        private readonly float bobAmplitude = 20f;      // how far up/down
        private readonly float bobFrequency = 2f;       // cycles per second

        // dash cycle timing
        private const double NormalDuration = 8.0;      // seconds before dash
        private const double DashDuration   = 2.0;      // dash length
        private double stateTimer = 0;
        private bool isDashing = false;

        // state
        private bool movingRight = true;
        private bool initialized = false;
        private float baseY;            // remembered starting Y
        private double bobTimer = 0;    // running time for sine

        public void Move(Entity entity, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // capture initial Y once
            if (!initialized)
            {
                baseY = entity.Position.Y;
                initialized = true;
            }

            // —— handle dash / normal timer
            stateTimer += dt;
            if (!isDashing && stateTimer >= NormalDuration)
            {
                isDashing = true;
                stateTimer = 0;
            }
            else if (isDashing && stateTimer >= DashDuration)
            {
                isDashing = false;
                stateTimer = 0;
            }

            // —— horizontal movement
            float speed = isDashing ? dashSpeed : patrolSpeed;
            entity.Position.X += speed * dt * (movingRight ? 1 : -1);

            // keep in screen bounds, flip direction
            float leftBound  = 0;
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

            // —— vertical bobbing
            bobTimer += dt;
            float offsetY = (float)Math.Sin(bobTimer * bobFrequency) * bobAmplitude;
            entity.Position.Y = baseY + offsetY;
        }
    }
}
