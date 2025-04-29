// Entities/BulletSpawners/ArchimedeanSpiralSpawner.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

namespace CompileAndConquer.Entities.BulletSpawners
{
    public class ArchimedeanSpiralSpawner : Entity
    {
        // configurable:
        public float A { get; set; }                // start radius 
        public float B { get; set; }                // radius growth factor
        public float AngularSpeed { get; set; }     // radians per second
        public float BulletInterval { get; set; }   // seconds between shots
        public int   MaxShots { get; set; }         
        public float BulletSpeed { get; set; }      

        // internal state:
        private Vector2 origin;
        private float theta;
        private float timeSinceLast;
        private int  shotsFired;
		private Color color = Color.Yellow;

        public ArchimedeanSpiralSpawner(
            Vector2 center,
            float a               = 20f,
            float b               =  5f,
            float angularSpeed    =  3f,
            float bulletInterval  =  0.02f,
            int   maxShots        =  60,
            float bulletSpeed     = 180f
        ) {
            this.origin        = center;
            this.A             = a;
            this.B             = b;
            this.AngularSpeed  = angularSpeed;
            this.BulletInterval= bulletInterval;
            this.MaxShots      = maxShots;
            this.BulletSpeed   = bulletSpeed;

            texture   = TextureHandler.Pixel;
            Scale     = 0f;   // invisible
            health    = 1;
            Position  = center;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // --- 1) move *this* spawner along the spiral ---
            theta += AngularSpeed * dt;                // advance angle
            float r = A + B * theta;                   // r = a + bθ
            Position = origin + new Vector2(
                (float)System.Math.Cos(theta),
                (float)System.Math.Sin(theta)
            ) * r;

            // --- 2) handle spawning bullets from *this* Position ---
            timeSinceLast += dt;
            if (shotsFired < MaxShots && timeSinceLast >= BulletInterval)
            {
                // one bullet toward player:
                Vector2 toPlayer = Player.Instance.Position - Position;
                if (toPlayer != Vector2.Zero) toPlayer.Normalize();
                EntityHandler.Add(new EnemyBullet(Position, new StraightBullet(toPlayer, BulletSpeed), color));

                // two flank streams ±120°:
                for (int i = 1; i < 3; i++)
                {
                    float ang = theta + MathHelper.TwoPi / 3 * i; 
                    Vector2 dir = new Vector2(
                        (float)System.Math.Cos(ang),
                        (float)System.Math.Sin(ang)
                    );
                    EntityHandler.Add(new EnemyBullet(Position, new StraightBullet(dir, BulletSpeed), color));
                }

                shotsFired++;
                timeSinceLast = 0f;
            }

            // die when done:
            if (shotsFired >= MaxShots)
                isAlive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // intentionally empty
        }
    }
}
