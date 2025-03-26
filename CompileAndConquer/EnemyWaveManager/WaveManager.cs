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
            public WaveEvent(IEnemyFactory spawnType, int spawnCount, double spawnTime, int duration, IEntry entryType, IExit exitType)
            {
                Type = spawnType;
                this.spawnTime = spawnTime;
                this.duration = duration;
                this.spawnCount = spawnCount;
                this.entryType = entryType;
                this.exitType = exitType;
                IsSpawned = false;
                EventEntities = new List<Entity>();
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
                        /*SpawnCount*/ 4, /*SpawnTime*/ 1, /*Duration*/ 30, new FromTopEntry(150, speed: 50f), new fromTopExit(targetY: -100, speed: 75f)),
                    new WaveEvent(new TypeAFactory(TextureHandler.TypeAEnemy, new Vector2(0,0), new LinearMovement(startRight: false)), 
                        /*SpawnCount*/ 4, /*SpawnTime*/ 1, /*Duration*/ 30, new FromTopEntry(150, speed: 50f), new fromTopExit(targetY: -100, speed: 75f)),
                    //new WaveEvent(new TypeBFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)),
                        ///*SpawnCount*/ 4, /*SpawnTime*/ 3, /*Duration*/ 30, new FromTopEntry(150), new fromTopExit(targetY: -100, speed: 75f)),
                    new WaveEvent(new MiniBossFactory(TextureHandler.MiniBoss, new Vector2(0,0), new LinearMovement(startRight: true)), 
                        /*SpawnCount*/ 1, /*SpawnTime*/ 15, /*Duration*/ 5, new FromTopEntry(150), new fromTopExit(targetY: -500, speed: 75f)),
                    new WaveEvent(new MiniBossFactory(TextureHandler.MiniBoss, new Vector2(0,0), new LinearMovement(startRight: true)), 
                        /*SpawnCount*/ 1, /*SpawnTime*/ 23, /*Duration*/ 8, new FromTopEntry(150), new fromTopExit(targetY: -500, speed: 75f)),
                    new WaveEvent(new TypeBFactory(TextureHandler.TypeBEnemy, new Vector2(0,0), new LinearMovement(startRight: true)), 
                        /*SpawnCount*/ 4, /*SpawnTime*/ 33, /*Duration*/ 10, new FromTopEntry(150, speed: 50f), new fromTopExit(targetY: -100, speed: 75f)),
                    new WaveEvent(new TypeAFactory(TextureHandler.TypeAEnemy, new Vector2(0,0), new LinearMovement(startRight: true)), 
                        /*SpawnCount*/ 4, /*SpawnTime*/ 35, /*Duration*/ 10, new FromTopEntry(150, speed: 50f), new fromTopExit(targetY: -100, speed: 75f))
                    /*
                    new WaveEvent(
                        new FinalBossFactory(TextureHandler.FinalBoss, new Vector2(0, 0)), 
                        spawnCount: 1,
                        spawnTime: 35,
                        duration: 9999,
                        new FromTopEntry(150, speed: 50f),
                        new fromTopExit(targetY: -500, speed: 75f)
                    )
                    */
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
                    EndEvent(waveEvent);
                }
                waveQueue.Dequeue();

                if (waveQueue.Count != 0)
                {
                    StartWave();
                }
            }
        }


        public void StartEvent(WaveEvent currentEvent)
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
                if (curEvent.IsSpawned && (waveTimer - curEvent.spawnTime) >= curEvent.duration)
                {
                    EndEvent(curEvent);
                    eventsToRemove.Add(curEvent);
                    Console.WriteLine("Event Ended");
                }
                
                if (!curEvent.IsSpawned && waveTimer >= curEvent.spawnTime)
                {
                    StartEvent(curEvent);
                    Console.WriteLine("Event Started");
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

                    if (entity.IsExiting && curEvent.exitType != null)
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

                enemy.Position = new Vector2(spacing * (i + 1), 0);
                enemy.IsEntering = true;

                // Assign exit behavior always
                enemy.ExitBehavior = currentEvent.exitType;

                EntityHandler.Add(enemy);
                currentEvent.EventEntities.Add(enemy);
            }

            currentEvent.IsSpawned = true;
        }
    }
}
