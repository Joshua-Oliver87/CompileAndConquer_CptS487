using System;
using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content.WaveManager
{
    public class SpiralEntryWave : IWave
    {
        private double duration;
        private double elapsed = 0;
        private bool spawned = false;
        private int numEnemies = 8;

        public SpiralEntryWave(double durationInSeconds)
        {
            duration = durationInSeconds;
        }

        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalSeconds;

            if (!spawned)
            {
                SpawnEnemies();
                spawned = true;
            }

            if (elapsed >= duration)
            {
                EntityHandler.ClearEnemies(); // Despawn if needed
            }
        }

        public bool IsComplete() => elapsed >= duration;

        private void SpawnEnemies()
        {
            float centerX = CompileAndConquer.ScreenSize.X / 2;
            float centerY = 100; // Start near top
            float radius = 200;

            for (int i = 0; i < numEnemies; i++)
            {
                double angle = i * (2 * Math.PI / numEnemies); // spread in a circle
                Vector2 pos = new Vector2(
                    centerX + (float)(radius * Math.Cos(angle)),
                    centerY + (float)(radius * Math.Sin(angle))
                );

                EntityHandler.Add(new TypeB(TextureHandler.TypeBEnemy, pos));
            }
        }
    }
}