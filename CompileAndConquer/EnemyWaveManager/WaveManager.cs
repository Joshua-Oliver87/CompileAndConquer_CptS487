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

        public class Wave
        {
            public List<WaveEvent> WaveEvents;
            //public double TriggerTime;
            public int waveDuration;

            public Wave()
            {
                WaveEvents = new List<WaveEvent>();
            }
        }

        /// <summary>
        /// Holds spawning information for individual entity spawns
        /// </summary>
        public class WaveEvent
        {
            public WaveEvent(IEnemyFactory spawnType, int spawnCount, double spawnTime, int duration, IEntry entryType, IExit exitType, bool shouldExpire = true)
            {
                this.Type = spawnType;
                this.spawnTime = spawnTime;
                this.duration = duration;
                this.spawnCount = spawnCount;
                this.entryType = entryType;
                this.exitType = exitType;
                this.IsSpawned = false;
                this.EventEntities = new List<Entity>();
                this.ShouldExpire = shouldExpire;
            }

            public double spawnTime;
            public int duration;
            public int spawnCount;
            public IEnemyFactory Type;
            public IEntry entryType;
            public IExit exitType;
            public bool IsSpawned;
            public bool ShouldExpire;
            public List<Entity> EventEntities;
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
                    /*SpawnCount*/ 5, /*SpawnTime*/ 1, /*Duration*/ 10, new FromTopEntry(150, speed: 50f), new fromTopExit(targetY: -100, speed: 75f)),
                    new WaveEvent(new TypeBFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)), //Tonight update spawn locations
                    /*SpawnCount*/ 4, /*SpawnTime*/ 11, /*Duration*/ 10, new FromTopEntry(150), new fromTopExit(targetY: -100, speed: 75f)),
                    new WaveEvent(new MiniBossFactory(TextureHandler.MiniBoss, new Vector2(0,0)), 
                    /*SpawnCount*/ 1, /*SpawnTime*/ 22, /*Duration*/ 12, new FromTopEntry(150), new fromTopExit(targetY: -100, speed: 75f)),
                    new WaveEvent(
                        new FinalBossFactory(TextureHandler.FinalBoss, new Vector2(0, 0)), 
                        spawnCount: 1,
                        spawnTime: 35,
                        duration: 9999,
                        new FromTopEntry(150, speed: 50f),
                        new fromTopExit(targetY: -100, speed: 75f),
                        shouldExpire: false 
                    )
                },
                waveDuration = 45
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
                    if (waveEvent.ShouldExpire)
                    {
                        EndEvent(waveEvent);
                    }
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
                currentEvent.IsSpawned = true;
                
            }
        }

        public void EndEvent(WaveEvent currentEvent)
        {
            if (!currentEvent.ShouldExpire)
                return;
            foreach (var entity in currentEvent.EventEntities)
            {
                if (entity.isAlive && !entity.IsExiting)
                {
                    entity.IsExiting = true;
                }
            }
        }


        public void UpdateWave(GameTime gameTime)
        {
            if (waveQueue.Count == 0)
                return;

            waveTimer += gameTime.ElapsedGameTime.TotalSeconds;
            List<WaveEvent> eventsToRemove = new();

            foreach (var curEvent in currentWave.WaveEvents)
            {
                if (!curEvent.IsSpawned && waveTimer >= curEvent.spawnTime)
                {
                    StartEvent(curEvent, currentWaveIndex);
                    Console.WriteLine("Event Started");
                }
                
                if (curEvent.ShouldExpire && curEvent.IsSpawned && (waveTimer - curEvent.spawnTime) >= curEvent.duration)
                {
                    EndEvent(curEvent);
                    eventsToRemove.Add(curEvent);
                    Console.WriteLine("Event Ended");
                }

                
                foreach (var entity in curEvent.EventEntities)
                {
                    if (entity.IsEntering && curEvent.entryType != null)
                    {
                        curEvent.entryType.Enter(entity, gameTime);

                        if (curEvent.entryType.HasEntered(entity))
                        {
                            entity.IsEntering = false;
                        }

                        continue; // prevent movement and exit logic during entry
                    }

                    if (curEvent.ShouldExpire && entity.IsExiting && curEvent.exitType != null)
                    {
                        curEvent.exitType.Exit(entity, gameTime);

                        if (entity.Position.Y < -entity.Size.Y || entity.Position.Y > CompileAndConquer.ScreenSize.Y)
                        {
                            EntityHandler.RemoveEntity(entity);
                            entity.isAlive = false;
                        }

                        continue;
                    }



                }
                
            }

            foreach (var eventToRemove in eventsToRemove)
            {
                currentWave.WaveEvents.Remove(eventToRemove);
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
            int totalEnemies = currentEvent.spawnCount;
            float spacing = CompileAndConquer.ScreenSize.X / (totalEnemies + 1);

            for (int i = 0; i < totalEnemies; i++)
            {
                var enemy = currentEvent.Type.CreateEnemy();

                // Assign new position after creation
                enemy.Position = new Vector2(spacing * (i + 1), 0);
                enemy.IsEntering = true;

                // Only assign exit behavior if it should expire
                if (currentEvent.ShouldExpire)
                    enemy.ExitBehavior = currentEvent.exitType;
                else
                    enemy.ExitBehavior = null;

                EntityHandler.Add(enemy);
                currentEvent.EventEntities.Add(enemy);
            }

            currentEvent.IsSpawned = true;
        }
    }
}
