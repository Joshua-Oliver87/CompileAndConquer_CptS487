using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entitites.Enemy_Factories;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content.WaveManager
{
    public class WaveManager
    {
        private Queue<Wave> waveQueue = new();
        private Wave currentWave;
        private double waveTimer = 0;
        private int currentWaveIndex = 0;

        public struct Wave
        {
            public List<WaveEvent> WaveEvents;
            //public double TriggerTime;
            public int waveDuration;
        }

        /// <summary>
        /// Holds spawning information for individual entity spawns
        /// </summary>
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

        /// <summary>
        /// Loading waves from hard coded values TODO: Load from JSON
        /// </summary>
        public void LoadWaves()
        {
            waveQueue.Enqueue(new Wave
            {
                WaveEvents = new List<WaveEvent>
                {
                    new WaveEvent(new TypeAFactory(TextureHandler.TypeAEnemy, new Vector2(0,0), new LinearMovement(startRight: true)), 
                    /*SpawnCount*/ 4, /*SpawnTime*/ 1, /*Duration*/ 10, new FromTopEntry(0), new fromTopExit(0)),
                    new WaveEvent(new TypeBFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)), //Tonight update spawn locations
                    /*SpawnCount*/ 4, /*SpawnTime*/ 11, /*Duration*/ 10, new FromTopEntry(0), new fromTopExit(0)),
                    new WaveEvent(new MiniBossFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)), 
                    /*SpawnCount*/ 1, /*SpawnTime*/ 15, /*Duration*/ 5, new FromTopEntry(0), new fromTopExit(0))
                },
                //TriggerTime = 1,
                waveDuration = 30
            });
            currentWave = waveQueue.Peek();
        }

        /// <summary>
        /// Start the next wave in the queue
        /// </summary>
        public void StartWave()
        {
            currentWave = waveQueue.Peek();
            waveTimer = 0;
        }

        /// <summary>
        /// End the current wave and any subevents still running, then start the next wave
        /// </summary>
        public void EndWave()
        {
            if (currentWave.WaveEvents.Count != 0)
            {
                foreach (var waveEvent in currentWave.WaveEvents)
                {
                    EndEvent(waveEvent);
                }
                waveQueue.Dequeue();

                if (waveQueue.Count != 0)
                {
                    StartWave();
                }
            }
        }


        public void StartEvent(WaveEvent currentEvent, int currentWaveIndex)
        {
            // Event duration timeout
            if (!currentEvent.IsSpawned)
            {
                SpawnEvent(currentEvent);
            }
        }

        public void EndEvent(WaveEvent currentEvent)
        {
            // Despawn all enemies
            foreach (var entity in currentEvent.EventEntities)
            {
                if (entity.isAlive)
                {
                    // TODO: Add exit movement and despawn
                }
            }
            currentWave.WaveEvents.Remove(currentEvent);
        }

        public void UpdateWave(GameTime gameTime)
        {
            // No more waves TODO: End Level
            if (waveQueue.Count == 0)
                return; 

            // Increment timer for wave
            waveTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Update each wave event
            foreach (var curEvent in currentWave.WaveEvents)
            {
                // Check if event is ready to spawn
                if (curEvent.IsSpawned == false && waveTimer >= curEvent.spawnTime)
                {
                    StartEvent(curEvent, currentWaveIndex);
                    Console.WriteLine("Event Started");
                }
                // Check if event is ready to despawn
                if (curEvent.IsSpawned == true && (waveTimer - curEvent.spawnTime) >= curEvent.duration)
                {
                    EndEvent(curEvent);
                    Console.WriteLine("Event Ended");
                }
            }

            if (waveTimer >= currentWave.waveDuration)
            {
                EndWave();
                waveTimer = 0;
                Console.WriteLine("Wave Ended");
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