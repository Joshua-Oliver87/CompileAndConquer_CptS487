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
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.Content.AttackPatterns;


namespace CompileAndConquer.Content.WaveManager
{
    public class WaveManager
    {
        private Queue<Wave> waveQueue = new();
        public Action Survived { get; set; }


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
            
            public float? startX = null; // Optional override
            public float? spacing = null;


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
            Random rng = new Random();
			var screenW = CompileAndConquer.ScreenSize.X;
			var columns = 4;
			var gap = screenW / (columns + 1);	// divide screen into columns to split enemy spawning
            waveQueue.Enqueue(new Wave
            {
                WaveEvents = new List<WaveEvent>
                {
                    new WaveEvent(
    					new TypeAFactory(
        					TextureHandler.TypeAEnemy,
        					new Vector2(0, 0),
        					() => new LinearMovement(startRight: false),
        					new StraightBullet(direction: new Vector2(0, 1), speed: 250f),
        					new TimedStraightAttack(0.5f, 250f)
    					),
    					spawnCount: columns,
    					spawnTime: 1,
    					duration: 15,
    					new SwoopEntry(target:new Vector2(50,180), duration:2.8f, amplitude:150f),
    					new SwoopExit(target:new Vector2(10,-100), duration:1.0f, amplitude:200f)
					) { startX = gap, spacing = gap },
					new WaveEvent(
    					new TypeAFactory(
        					TextureHandler.TypeAEnemy,
        					new Vector2(0, 0),
        					() => new LinearMovement(startRight: true),
        					new StraightBullet(direction: new Vector2(0, 1), speed: 250f),
        					new TimedStraightAttack(0.5f, 250f)
    					),
    					spawnCount: columns,
    					spawnTime: 1,
    					duration: 15,
    					new SwoopEntry(target:new Vector2(50,180), duration:2.8f, amplitude:150f),
    					new SwoopExit(target:new Vector2(10,-100), duration:1.0f, amplitude:200f)
					) { startX = screenW - gap*columns, spacing = gap },
                    new WaveEvent(new TypeAFactory(TextureHandler.TypeAEnemy, new Vector2(0,0), () => new LinearMovement(startRight: false), new StraightBullet(direction: new Vector2(0, 1), speed: 250f), new RepeatingSpiralAttack()), 
                        /*SpawnCount*/ 1, /*SpawnTime*/ 1, /*Duration*/ 30, new FromTopEntry(150, speed: 50f), new fromTopExit(targetY: -100, speed: 75f)),
                    //new WaveEvent(new TypeBFactory(TextureHandler.TypeBEnemy, new Vector2(0,0)),
                        ///*SpawnCount*/ 4, /*SpawnTime*/ 3, /*Duration*/ 30, new FromTopEntry(150), new fromTopExit(targetY: -100, speed: 75f)),
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                new Vector2(0,0),
                                new CurvedMovement(
                                    horizontalAmplitude: 200f,
                                    verticalAmplitude: 50f,
                                    horizontalFrequency: 0.7f,
                                    verticalFrequency: 1.3f
                                ),
                                new HomingBullet()
                            ), 
                            /*SpawnCount*/ 1,
                            /*SpawnTime*/ 23,
                            /*Duration*/ 10,
                            new FromTopEntry(150),
                            new fromTopExit(targetY: -500, speed: 75f)
                        ),

                    new WaveEvent(new TypeBFactory(TextureHandler.TypeBEnemy, new Vector2(0,0), new ZigZagMovement(
                        leftBound: 50,
                        rightBound: 950,
                        topBound: 100,
                        bottomBound: 300,
                        horizontalSpeed: 120f,
                        verticalSpeed: 60f), new HomingBullet()), 
                        /*SpawnCount*/ 4, /*SpawnTime*/ 33, /*Duration*/ 10, new FromTopEntry(100, speed: 50f), new fromTopExit(targetY: -100, speed: 75f)),
                    new WaveEvent(new TypeAFactory(TextureHandler.TypeAEnemy, new Vector2(0,0),() => new ZigZagMovement(
                            leftBound: 50,
                            rightBound: 950,
                            topBound: 100 + rng.Next(0,50),
                            bottomBound: 300+ rng.Next(0,50),
                            horizontalSpeed: 120f+ rng.Next(0,30),
                            verticalSpeed: 60f+ rng.Next(0,20)), new StraightBullet(direction: new Vector2(0, 1), speed: 250f), null), 
                        /*SpawnCount*/ 4, /*SpawnTime*/ 35, /*Duration*/ 10, new FromTopEntry(150, speed: 50f), new fromTopExit(targetY: -100, speed: 75f)),
                    
                    new WaveEvent(
                        new FinalBossFactory(
                            TextureHandler.FinalBoss,
                            new Vector2(0, 0),
                            new FinalBossMovement(horizontalSpeed: 60f, verticalAmplitude: 40f, verticalFrequency: 1.5f), new HomingBullet()),
                        spawnCount: 1,
                        spawnTime: 46,
                        duration: 30,
                        new FromTopEntry(150, speed: 50f),
                        new fromTopExit(targetY: -500, speed: 75f)
                    )
                },
                waveDuration = 80
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
            if (waveQueue.Count == 0) //this never happens
            {       
                Console.WriteLine("wavequee empty");
                Survived?.Invoke();
                return;
            }

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
                if (currentWave.WaveEvents.Count == 0 && this.waveQueue.Count == 1)
                {
                    //player wins
                    Survived?.Invoke();
                }
                
            }
        }
        private void SpawnEvent(WaveEvent currentEvent)
        {
            int totalEnemies = currentEvent.spawnCount;
            float defaultSpacing = CompileAndConquer.ScreenSize.X / (totalEnemies + 1);
            float spacing = currentEvent.spacing ?? defaultSpacing;
            float startX = currentEvent.startX ?? spacing;

            for (int i = 0; i < totalEnemies; i++)
            {
                var enemy = currentEvent.Type.CreateEnemy();

                enemy.Position = new Vector2(startX + spacing * i, 0);
                enemy.IsEntering = true;
                enemy.ExitBehavior = currentEvent.exitType;

                EntityHandler.Add(enemy);
                currentEvent.EventEntities.Add(enemy);
            }

            currentEvent.IsSpawned = true;
        }
    }
}
