using Microsoft.Xna.Framework;
using System;

namespace CompileAndConquer.Content
{
        public class ZigZagMovement : IMovementStrategy 
	{
        private bool movingRight = true;
        private bool movingDown  = true;
        private float leftBound, rightBound, topBound, bottomBound;
        private float horizontalSpeed, verticalSpeed;
    
        public ZigZagMovement(
            float leftBound, float rightBound,
            float topBound, float bottomBound,
            float horizontalSpeed = 150f, float verticalSpeed = 75f
        ) {
            this.leftBound       = leftBound;
            this.rightBound      = rightBound;
            this.topBound        = topBound;
            this.bottomBound     = bottomBound;
            this.horizontalSpeed = horizontalSpeed;
            this.verticalSpeed   = verticalSpeed;
        }
    
        public void Move(Entity entity, GameTime gameTime) {
            var enemy = (Enemy)entity;
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var pos = enemy.Position;
    
            // horizontal
            pos.X += (movingRight ? horizontalSpeed : -horizontalSpeed) * dt;
            if (pos.X <= leftBound)  { pos.X = leftBound;  movingRight = true;  }
            else if (pos.X >= rightBound) { pos.X = rightBound; movingRight = false; }
    
            // vertical
            pos.Y += (movingDown ? verticalSpeed : -verticalSpeed) * dt;
            if (pos.Y <= topBound)    { pos.Y = topBound;    movingDown = true;  }
            else if (pos.Y >= bottomBound) { pos.Y = bottomBound; movingDown = false; }
    
            enemy.Position = pos;
        }
    }
}
