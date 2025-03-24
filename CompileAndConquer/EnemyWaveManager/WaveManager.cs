using System;
using System.Collections.Generic;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entitites.Enemy_Factories;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content.WaveManager
{
    public class WaveManager
    {
        private List<Wave> waves = new();
        private int currentWaveIndex = 0;
        private int currentEventIndex = 0;
        private double waveTimer = 0;

        public class Wave
        {
            public List<WaveEvent> WaveEvents;
            public double TriggerTime;
            public int waveDuration;

            public Wave()
            {
                WaveEvents = new List<WaveEvent>();
            }
        }

        public class WaveEvent
        {
            public WaveEvent(IEnemyFactory spawnType, int spawnCount, double spawnTime, int duration, IEntry entryType, IExit exitType)
            {
                this.Type = spawnType;
                this.spawnTime = spawnTime;
                this.duration = duration;
                this.spawnCount = spawnCount;
                this.entryType = entryType;
                this.exitType = exitType;
                this.IsSpawned = false;
                this.EventEntities = new List<Entity>();
            }

            public double spawnTime;
            public int duration;
            public int spawnCount;
            public IEnemyFactory Type;
            public IEntry entryType;
            public IExit exitType;
            public bool IsSpawned;
            public List<Entity> EventEntities;
        }

        public void LoadWaves()
        {
            waves.Add(new Wave
            {
                WaveEvents = new List<WaveEvent>
                {
                    new WaveEvent(new TypeAFactory(TextureHandler.TypeAEnemy, new Vector2(0,0), new LinearMovement(startRight: true)), 4, 1, 10, new FromTopEntry(0), new fromTopExit(0)),
                    new WaveEvent(new TypeBFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)), 4, 11, 10, new FromTopEntry(0), new fromTopExit(0)),
                    new WaveEvent(new MiniBossFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)), 1, 15, 5, new FromTopEntry(0), new fromTopExit(0))
                },
                TriggerTime = 1,
                waveDuration = 30
            });
        }

        public void StartWave()
        {
            if (waves.Count == 0)
                return;

            currentEventIndex = 0;
        }

        public void EndWave(WaveEvent waveEvent)
        {
            // Add logic later
        }

        public void StartWaveEvent(WaveEvent currentEvent, int currentWaveIndex)
        {
            if (!currentEvent.IsSpawned)
            {
                SpawnEvent(currentEvent);
            }
        }

        public void EndWaveEvent(WaveEvent currentEvent)
        {
            foreach (var entity in currentEvent.EventEntities)
            {
                EntityHandler.RemoveEntity(entity); // Fix: this is now valid
            }
        }

        public void UpdateWave(GameTime gameTime)
        {
            if (waves.Count == 0)
                return;

            waveTimer += gameTime.ElapsedGameTime.TotalSeconds;

            var currentWave = waves[currentWaveIndex];
            foreach (var waveEvent in currentWave.WaveEvents)
            {
                if (!waveEvent.IsSpawned && waveTimer >= waveEvent.spawnTime)
                {
                    StartWaveEvent(waveEvent, currentWaveIndex);
                    waveEvent.IsSpawned = true;
                }

                // You could also check here if waveEvent.duration has passed and then call EndWaveEvent(waveEvent)
            }

            if (waveTimer >= currentWave.waveDuration)
            {
                currentWaveIndex++;
                waveTimer = 0;
            }
        }

        private void SpawnEvent(WaveEvent currentEvent)
        {
            for (int i = 0; i < currentEvent.spawnCount; i++)
            {
                var enemy = currentEvent.Type.CreateEnemy();
                EntityHandler.Add(enemy);
                currentEvent.EventEntities.Add(enemy);
            }
        }
    }
}
