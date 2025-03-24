using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entitites.Enemy_Factories;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content.WaveManager
{
    public class WaveManager
    {
        private List<IWave> waves = new();
        private int currentWaveIndex = 0;
        private int currentEventIndex = 0;
        private double waveTimer = 0;

        public struct Wave
        {
            public List<WaveEvent> WaveEvents;
            public double TriggerTime;
            public int waveDuration;
        }

        public struct WaveEvent
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
            public IEnemyFactory Type; // Modularize Factory to EntityFactory later for more flexibility  
            public IEntry entryType;
            public IExit exitType;
            public bool IsSpawned = false;
            public List<Entity> EventEntities;
            // Need to generalize to include text/powerup spawns  
        }

        public void AddWave(IWave wave)
        {
            waves.Add(wave);
        }

        public void LoadWaves()
        {
            waves.Add(new Wave
            {
                WaveEvents = new List<WaveEvent>
                {
                    new WaveEvent(new TypeAFactory(TextureHandler.TypeAEnemy, new Vector2(0,0), new LinearMovement(startRight: true)), /*SpawnCount*/ 4, /*SpawnTime*/ 1, /*Duration*/ 10, new FromTopEntry(0), new fromTopExit(0)),
                    new WaveEvent(new TypeBFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)), /*SpawnCount*/ 4, /*SpawnTime*/ 11, /*Duration*/ 10, new FromTopEntry(0), new fromTopExit(0)),
                    new WaveEvent(new MiniBossFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)), /*SpawnCount*/ 1, /*SpawnTime*/ 15, /*Duration*/ 5, new FromTopEntry(0), new fromTopExit(0));
                },
                TriggerTime = 1,
                waveDuration = 30
            });
            return;
        }

        public void StartWave()
        {
            if (waves.Count == 0)
                return;

            currentEventIndex = 0;
        }

        public void EndWave(WaveEvent)
        {

        }

        public void StartWaveEvent(WaveEvent currentEvent, int currentWaveIndex)
        {


            // Event duration timeout
            if (!currentEvent.IsSpawned)
            {
                SpawnEvent(currentEvent);
            }
        }

        public void EndWaveEvent(WaveEvent currentEvent)
        {
            // Despawn all enemies
            foreach (var entity in currentEvent.EventEntities)
            {
                if entity.
            }
        }

        public void UpdateWave(GameTime gameTime)
        {
            if (waves.Count == 0)
                return; // No more waves

            // Wave
            waveTimer += gameTime.ElapsedGameTime.TotalSeconds; // Increment timer
            var currentWave = waves[currentWaveIndex]; 

            currentWave.Update(gameTime);

            if (currentWave.IsComplete())
            {
                currentWaveIndex++;
                waveTimer = 0;
            }
        }

        private void SpawnEvent(WaveEvent currentEvent)
        {
            for (int i = 0; i < currentEvent.spawnCount; i++)
            {
                var position = new Vector2(200 + i * 60, -100);
                var enemy = currentEvent.Type.CreateEnemy();
                EntityHandler.Add(enemy);
                currentEvent.EventEntities.Add(enemy);
            }
        }

    }
}