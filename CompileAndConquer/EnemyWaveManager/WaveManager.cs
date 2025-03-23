using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content.WaveManager
{
    public class WaveManager
    {
        private List<IWave> waves = new();
        private int currentWaveIndex = 0;
        private double waveTimer = 0;

        public void AddWave(IWave wave)
        {
            waves.Add(wave);
        }

        public void LoadWaves()
        {

        }

        public void UpdateWave(GameTime gameTime)
        {
            if (currentWaveIndex >= waves.Count)
                return; // No more waves

            waveTimer += gameTime.ElapsedGameTime.TotalSeconds;
            var currentWave = waves[currentWaveIndex];

            currentWave.Update(gameTime);

            if (currentWave.IsComplete())
            {
                currentWaveIndex++;
                waveTimer = 0;
            }
        }

        public struct WaveEvent
        {
            public int EnemyCount;
            public Enemy EnemyType;
            // All neccesary input for enemy builder
            // Need to generalize to include text/powerup spawns
        }

        public struct Wave
        {
            public List<WaveEvent> WaveEvents;
            public int waveDuration;
        }
    }
}