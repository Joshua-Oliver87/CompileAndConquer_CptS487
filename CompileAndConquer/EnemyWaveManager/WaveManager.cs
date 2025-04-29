using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entities.Enemy_Factories;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.Content.AttackPatterns;
using CompileAndConquer.SpawnPatterns;
using CompileAndConquer.Content.EnemyWaveManager;




namespace CompileAndConquer.Content.WaveManager
{
    public class WaveManager
    {
        private Queue<Wave> waveQueue = new();
        public Action Survived { get; set; }


        private Wave currentWave;
       // private double waveTimer = 0;

        public class Wave
        {
            // Each step is a group of events that spawn together
            public List<List<WaveEvent>> Steps { get; set; }
            public int CurrentStep { get; set; }
        
            public Wave()
            {
                Steps = new List<List<WaveEvent>>();
                CurrentStep = 0;
            }
        }

		private readonly List<BossPhase> bossPhases = new List<BossPhase>
        {
            new BossPhase {
                Movement = new  miniBossMovement(),
                Attack = new AlternatingAttackPattern(
                    pauseBetweenAttacks: 12f,
                    new TimedStraightAttack(dir => new HomingBullet()),
                    new ArcAttackPattern(dir => new StraightBullet(dir, 250f), 0.5f)
                ),
                HpThreshold = 2000
            },
            new BossPhase {
                Movement = new FinalBossMovement(),
                Attack = new AlternatingAttackPattern(
                    pauseBetweenAttacks: 15f,
                    new ConcentricCircleAttack(),
                    new ThreeWaySpiralAttack()
                ),
                HpThreshold = 1000
            },
            new BossPhase {
                Movement = new  miniBossMovement(),
                Attack = new AlternatingAttackPattern(
                    pauseBetweenAttacks: 8f,
                    new RepeatingSpiralAttack(),
                    new ConcentricCircleAttack()
                ),
                HpThreshold = 500
            }
        };


		public enum WaveEventState
        {
            NotStarted,
            Entering,
            Active,
            Exiting,
            Complete
        }

        /// <summary>
        /// Holds spawning information for individual entity spawns
        /// </summary>
        public class WaveEvent
        {
            public IEnemyFactory Type { get; }
            public int spawnCount { get; }
            public double spawnTime { get; }
            public int duration { get; }
            public ISpawnPattern SpawnPattern { get; set; }
            public float? startX { get; set; }
            public float? spacing { get; set; }
            public bool IsSpawned { get; set; }
            public List<Entity> EventEntities { get; }

			public WaveEventState State { get; set; }
            public double StateTimer { get; set; }

            private readonly Func<Vector2,IEntry> _entryFactory;
            private readonly Func<Vector2, IExit>  _exitFactory;

            public WaveEvent(
                IEnemyFactory spawnType,
                int spawnCount,
                int duration,
                Func<Vector2,IEntry> entryFactory,
                Func<Vector2,IExit> exitFactory
            )
            {
                Type           = spawnType;
                this.spawnCount = spawnCount;
                this.duration   = duration;
                _entryFactory  = entryFactory;
                _exitFactory   = exitFactory;
                SpawnPattern   = null;
                IsSpawned      = false;
                EventEntities  = new List<Entity>();
				State = WaveEventState.NotStarted;
                StateTimer = 0;
            }

            public IEntry CreateEntry(Vector2 finalPos) => _entryFactory(finalPos);
            public IExit  CreateExit(Vector2 finalPos)  => _exitFactory(finalPos);
        }


        /// <summary>
        /// Loading waves from hard coded values TODO: Load from JSON
        /// </summary>
        public void LoadWaves()
        {
            waveQueue.Clear();
            var screenW = CompileAndConquer.ScreenSize.X;
        
            waveQueue.Enqueue(new Wave
            {
                Steps = new List<List<WaveEvent>>
                {
                    
                    
                    //  4 grunts move left-to-right
                    
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                new Vector2(0,0),
                                () => new LinearMovement(startRight: true),
                                () => new TriangleAttack(rows: 3, spacing: 40f)
                            ),
                            spawnCount: 4,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 120f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
                    
                    //  4 grunts move right-to-left
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                new Vector2(0,0),
                                () => new LinearMovement(startRight: false),
                                () => new TimedStraightAttack(dir => new StraightBullet(dir, 250f), 0.5f)
                            ),
                            spawnCount: 4,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 220f)
                        ) { SpawnPattern = new LineSpawnPattern(y:150f) }
                    },
        
                    //  4 grunts move left-to-right
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                new Vector2(0,0),
                                () => new LinearMovement(startRight: true),
                                () => new TimedStraightAttack(dir => new StraightBullet(dir, 250f), 0.5f)
                            ),
                            spawnCount: 4,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Right, 120f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
        
                    //  4 grunts move right-to-left
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                new Vector2(0,0),
                                () => new LinearMovement(startRight: false),
                                () => new TimedStraightAttack(dir => new StraightBullet(dir, 250f), 0.5f)
                            ),
                            spawnCount: 4,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 120f)
                        ) { SpawnPattern = new LineSpawnPattern(y:150f) }
                    },
        
                    // First fairy miniboss
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                new Vector2(0,0),
                                () => new CurvedMovement(
                                    horizontalAmplitude: 200f,
                                    verticalAmplitude: 50f,
                                    horizontalFrequency: 0.7f,
                                    verticalFrequency: 1.3f
                                ),
                                () => new TriangleAttack(rows: 3, spacing: 40f)),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 220f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
        
                    //  Second fairy miniboss
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                new Vector2(0,0),
                                () => new CurvedMovement(
                                    horizontalAmplitude: 200f,
                                    verticalAmplitude: 50f,
                                    horizontalFrequency: 0.7f,
                                    verticalFrequency: 1.3f
                                ),
                                () => new TriangleAttack(rows: 3, spacing: 40f)
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
        
                    // 2 lines of grunts in parallel (horizontal lines)
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                new Vector2(0,0),
                                () => new StationaryMovement(),
                                () => new CircleAttack(24,30f, dir => new StraightBullet(dir, 200f))
                            ),
                            spawnCount: 4,
                            duration: 2,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:230f) },
        
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                new Vector2(0,0),
                                () => new StationaryMovement(),
                                () => new CircleAttack(24,30f, dir => new StraightBullet(dir, 200f))
                            ),
                            spawnCount: 4,
                            duration: 2,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:180f) }
                    },
        
                    // Midboss attack phase (alternating arc & circle)
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                new Vector2(0,0),
                                () => new miniBossMovement(),
                                () => new AlternatingAttackPattern(
                                    pauseBetweenAttacks: 5f,
                                    new ArcAttackPattern(dir => new StraightBullet(dir, 200f), 1f),
                                    new ConcentricCircleAttack()
                                )
                            ),
                            spawnCount: 1,
                            duration: 25,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },


        
					// SECOND ROUND AFTER MINIBOSS
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: false),
                                () => new TriangleAttack(rows: 3, spacing: 40f)
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Right, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Left, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) },
            
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: true),
                                () => new TriangleAttack(rows: 3, spacing: 40f)
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Right, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) },
						new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: true),
                                () => new noAttack()
                            ),
                            spawnCount: 5,
                            duration: 4,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Left, 220f)
                        ) { SpawnPattern = new LineSpawnPattern(y:100) },
						new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: false),
                                () => new noAttack()
                            ),
                            spawnCount: 5,
                            duration: 4,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Right, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Right, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:150) }
                    },
            
            
                    // Two horizontal lines (both at once, parallel)
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                Vector2.Zero,
                                () => new StationaryMovement(),
                                () => new CircleAttack(24,30f, dir => new StraightBullet(dir, 200f))
                            ),
                            spawnCount: 4,
                            duration: 2,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) },
            
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                Vector2.Zero,
                                () => new StationaryMovement(),
                                () => new CircleAttack(24,30f, dir => new StraightBullet(dir, 200f))
                            ),
                            spawnCount: 4,
                            duration: 2,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 100f)
                        ) { SpawnPattern = new LineSpawnPattern(y:180f) }
                    },

					//REPEAT ABOVE 2 WAVEEVENTS
					new List<WaveEvent>
                    {
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: false),
                                () => new TriangleAttack(rows: 3, spacing: 40f)
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Right, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Left, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) },
            
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: true),
                                () => new TriangleAttack(rows: 3, spacing: 40f)
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Right, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) },
						new WaveEvent(
                            new TypeBFactory(
                                TextureHandler.TypeBEnemy,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: true),
                                () => new noAttack()
                            ),
                            spawnCount: 5,
                            duration: 4,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Left, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:100) },
						new WaveEvent(
                            new TypeBFactory(
                                TextureHandler.TypeBEnemy,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: false),
                                () => new noAttack()
                            ),
                            spawnCount: 5,
                            duration: 4,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Right, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Right, 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:150) }
                    },
            
            
                    // Two horizontal lines (both at once, parallel)
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeBFactory(
                                TextureHandler.TypeBEnemy,
                                Vector2.Zero,
                                () => new StationaryMovement(),
                                () => new CircleAttack(24,30f, dir => new StraightBullet(dir, 200f))
                            ),
                            spawnCount: 4,
                            duration: 2,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 100f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) },
            
                        new WaveEvent(
                            new TypeBFactory(
                                TextureHandler.TypeBEnemy,
                                Vector2.Zero,
                                () => new StationaryMovement(),
                                () => new CircleAttack(24,30f, dir => new StraightBullet(dir, 200f))
                            ),
                            spawnCount: 4,
                            duration: 2,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Top, 100f)
                        ) { SpawnPattern = new LineSpawnPattern(y:180f) }
                    },
                    
                
                    // grunts in pyramid on left, move right and attack
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeBFactory(
                                TextureHandler.TypeBEnemy,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: false),
                                () => new TriangleAttack(rows: 3, spacing: 40f)
                            ),
                            spawnCount: 6,
                            duration: 16,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Left, 120f)
                        ) { SpawnPattern = new PyramidSpawnPattern(rows: 3, spacing: 40f, y: 200f) },

						new WaveEvent(
                            new TypeBFactory(
                                TextureHandler.TypeBEnemy,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: true),
                                () => new TimedStraightAttack(dir => new StraightBullet(dir, 300f), 0.5f)
                            ),
                            spawnCount: 6,
                            duration: 6,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Right, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Right, 120f)
                        ) { SpawnPattern = new PyramidSpawnPattern(rows: 3, spacing: 40f, y: 200f) },
                    },
                    
                    // FINAL BOSS:
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new FinalBossFactory(
                                TextureHandler.FinalBoss,
                                Vector2.Zero,           
                                bossPhases,             
                                bossPhases[0].Attack    // phase 1 A.P.
                            ),
                            spawnCount: 1,
                            duration: 90,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(
                                CardinalDirection.Left, 120f)
                        ) { SpawnPattern = new LineSpawnPattern(y: 100) },
                    }
                }
            });
            currentWave = waveQueue.Peek();
        }




        /// <summary>
        /// Start the next wave in the queue
        /// </summary>
        public void StartWave()
        {
            currentWave = waveQueue.Peek();
        }

        /// <summary>
        /// End the current wave and any subevents still running, then start the next wave
        /// </summary>
        public void EndWave()
        {
            if (currentWave == null || currentWave.CurrentStep >= currentWave.Steps.Count)
                return;
        
            foreach (var waveEvent in currentWave.Steps[currentWave.CurrentStep])
            {
                EndEvent(waveEvent);
            }
        
            // Advance to next step (next set of parallel events)
            currentWave.CurrentStep++;
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
            // If there are no waves at all, trigger Survived immediately
            if (waveQueue.Count == 0)
            {
                Survived?.Invoke();
                return;
            }
        
            //  grab the current wave
            if (currentWave == null)
                currentWave = waveQueue.Peek();
        
            // Run the current step
            var parallelEvents = currentWave.Steps[currentWave.CurrentStep];
        
            // Spawn & advance each event in this step
            foreach (var waveEvent in parallelEvents)
            {
                if (waveEvent.State == WaveEventState.NotStarted)
                {
                    SpawnEvent(waveEvent);
                    waveEvent.State = WaveEventState.Entering;
                    waveEvent.StateTimer = 0;
                }
        
                // Advance its timer and state machine
                waveEvent.StateTimer += gameTime.ElapsedGameTime.TotalSeconds;
                switch (waveEvent.State)
                {
                    case WaveEventState.Entering:
                        if (waveEvent.EventEntities.All(e => !e.IsEntering))
                        {
                            waveEvent.State = WaveEventState.Active;
                            waveEvent.StateTimer = 0;
                        }
                        break;
        
                    case WaveEventState.Active:
                        if (waveEvent.StateTimer >= waveEvent.duration
                            || waveEvent.EventEntities.All(e => !e.isAlive))
                        {
                            // kick off the exit phase
                            foreach (var e in waveEvent.EventEntities.Where(e => e.isAlive && !e.IsExiting))
                                e.IsExiting = true;
                            waveEvent.State = WaveEventState.Exiting;
                            waveEvent.StateTimer = 0;
                        }
                        break;
        
                    case WaveEventState.Exiting:
                        if (waveEvent.EventEntities.All(e => !e.isAlive))
                        {
                            waveEvent.State = WaveEventState.Complete;
                        }
                        break;
                }
        
                //update every entity in that event
                foreach (var e in waveEvent.EventEntities.ToList())
                    e.Update(gameTime);
            }
        
            // If ALL events in this step finished go to next step
            if (parallelEvents.All(ev => ev.State == WaveEventState.Complete))
            {
                currentWave.CurrentStep++;
            }
        
            //  check whether the wave itself is done
            if (currentWave.CurrentStep >= currentWave.Steps.Count)
            {
                waveQueue.Dequeue();
        
                // if that was the last wave, we win
                if (waveQueue.Count == 0)
                {
                    Survived?.Invoke();
                }
                else
                {
                    //else , start the next wave
                    currentWave = waveQueue.Peek();
                }
            }
        }


        private void SpawnEvent(WaveEvent ev)
        {
            int total = ev.spawnCount;
            var screen = CompileAndConquer.ScreenSize;

            for (int i = 0; i < total; i++)
            {
                var enemy = ev.Type.CreateEnemy();
                Vector2 finalPos = ev.SpawnPattern.GetPosition(i, total, screen);

                enemy.IsEntering = true;
                enemy.EntryBehavior = ev.CreateEntry(finalPos);
                enemy.ExitBehavior = ev.CreateExit(finalPos);

                EntityHandler.Add(enemy);
                ev.EventEntities.Add(enemy);
            }
            ev.IsSpawned = true;
        }

    }
}
