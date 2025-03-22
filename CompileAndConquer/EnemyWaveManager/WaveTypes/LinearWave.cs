using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;


namespace CompileAndConquer.Content.WaveManager
{
    public class LinearWave : IWave
    {
        
        private List<(Enemy enemy, IEntry entry)> enteringEnemies = new();
        private double duration;
        private double elapsed = 0;
        private bool spawned = false;

        public LinearWave(double durationInSeconds)
        {
            duration = durationInSeconds;
        }

        public void Update(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.TotalSeconds;

            // 💥 This is missing in your current code
            if (!spawned)
            {
                SpawnEnemies();
                spawned = true;
            }

            foreach (var (enemy, entry) in enteringEnemies)
            {
                if (!entry.HasEntered(enemy))
                    entry.Enter(enemy, gameTime);
            }

            enteringEnemies.RemoveAll(e => e.entry.HasEntered(e.enemy));

            if (elapsed >= duration)
            {
                EntityHandler.ClearEnemies();
            }
        }
        public bool IsComplete() => elapsed >= duration;

        private void SpawnEnemies()
        {
            for (int i = 0; i < 10; i++)
            {
                var position = new Vector2(200 + i * 60, -100);
                var enemy = new TypeA(TextureHandler.TypeAEnemy, position, new LinearMovement(startRight: true));
                var entry = new FromTopEntry(100 + i * 50);
                enteringEnemies.Add((enemy, entry));
                EntityHandler.Add(enemy);
            }
        }

        private void DespawnAllEnemies()
        {
            EntityHandler.ClearEnemies();
        }
    }
}