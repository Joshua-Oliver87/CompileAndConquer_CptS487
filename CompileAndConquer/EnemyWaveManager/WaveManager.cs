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
                Movement = new FinalBossMovement(),
                Attack = new AlternatingAttackPattern(
                    pauseBetweenAttacks: 15f,
                    new RepeatingSpiralAttack(),
                    new ConcentricCircleAttack()
                ),
                HpThreshold = 2000
            },
            new BossPhase {
                Movement = new ZigZagMovement(leftBound:   0f, rightBound:  CompileAndConquer.ScreenSize.X, topBound:    100f, bottomBound: CompileAndConquer.ScreenSize.Y - 100f, horizontalSpeed: 150f, verticalSpeed:   75f),
                Attack = new AlternatingAttackPattern(
                    pauseBetweenAttacks: 5f,
                    new RepeatingSpiralAttack(),
                    //new RepeatingSpiralAttack()
                    new ArcAttackPattern(dir => new StraightBullet(dir, 200f), 1f)
                ),
                HpThreshold = 800
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
            private readonly Func<IExit>  _exitFactory;

            public WaveEvent(
                IEnemyFactory spawnType,
                int spawnCount,
                int duration,
                Func<Vector2,IEntry> entryFactory,
                Func<IExit> exitFactory
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
            public IExit  CreateExit()  => _exitFactory();
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
                    // STEP 1: 4 grunts move left-to-right
/*
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                new Vector2(0,0),
                                () => new LinearMovement(startRight: true),
                                () => new TriangleAttack(8,10f,2.0, dir => new StraightBullet(new Vector2(0,1), 350f))
                            ),
                            spawnCount: 4,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory: () => new DirectionalExit(CardinalDirection.Top, new Vector2(-100f,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
        
                    // STEP 2: 4 grunts move right-to-left
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Top, new Vector2(-100f,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:150f) }
                    },
        
                    // STEP 3: 4 grunts move left-to-right
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Right, new Vector2(-100f,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
        
                    // STEP 4: 4 grunts move right-to-left
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Top, new Vector2(-100f,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:150f) }
                    },
        
                    // STEP 5: First fairy miniboss
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
                                () => new TriangleAttack(8,10f,2.0, dir => new StraightBullet(new Vector2(0,1), 350f))
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory: () => new DirectionalExit(CardinalDirection.Top, new Vector2(-100f,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
        
                    // STEP 6: Second fairy miniboss
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
                                () => new TriangleAttack(8,10f,2.0, dir => new StraightBullet(new Vector2(0,1), 350f))
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory: () => new DirectionalExit(CardinalDirection.Top, new Vector2(-100f,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
        
                    // STEP 7: 2 lines of grunts in parallel (horizontal lines)
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Top, new Vector2(-100f,220), 320f)
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Top, new Vector2(-100f,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:180f) }
                    },
        
                    // STEP 8: Midboss attack phase (alternating arc & circle)
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                new Vector2(0,0),
                                () => new miniBossMovement(),
                                () => new AlternatingAttackPattern(
                                    switchInterval: 2f,
                                    new ArcAttackPattern(dir => new StraightBullet(dir, 200f), 1f),
                                    new ConcentricCircleAttack(cooldown: 1.2f)
                                )
                            ),
                            spawnCount: 1,
                            duration: 25,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory: () => new DirectionalExit(CardinalDirection.Top, new Vector2(-100f,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },


        
					// SECOND WAVE AFTER MINIBOSS
                    new List<WaveEvent>
                    {
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: false),
                                () => new TriangleAttack(8,10f,2.0, dir => new StraightBullet(new Vector2(0,1), 350f))
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Right, target: finalPos, speed: 120f),
                            exitFactory: () => new DirectionalExit(CardinalDirection.Left, new Vector2(CompileAndConquer.ScreenSize.X+50,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) },
            
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: true),
                                () => new TriangleAttack(8,10f,2.0, dir => new StraightBullet(new Vector2(0,1), 350f))
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory: () => new DirectionalExit(CardinalDirection.Right, new Vector2(-100f,220), 320f)
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Left, new Vector2(-100f,220), 320f)
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Right, new Vector2(CompileAndConquer.ScreenSize.X+50,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:150) }
                    },
            
            
                    // STEP: Two horizontal lines (both at once, parallel)
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
                            exitFactory: () => new FromTopExit(targetY: -100, speed: 100f)
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
                            exitFactory: () => new FromTopExit(targetY: -100, speed: 100f)
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
                                () => new TriangleAttack(8,10f,2.0, dir => new StraightBullet(new Vector2(0,1), 350f))
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Right, target: finalPos, speed: 120f),
                            exitFactory: () => new DirectionalExit(CardinalDirection.Left, new Vector2(CompileAndConquer.ScreenSize.X+50,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) },
            
                        new WaveEvent(
                            new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                Vector2.Zero,
                                () => new LinearMovement(startRight: true),
                                () => new TriangleAttack(8,10f,2.0, dir => new StraightBullet(new Vector2(0,1), 350f))
                            ),
                            spawnCount: 1,
                            duration: 5,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory: () => new DirectionalExit(CardinalDirection.Right, new Vector2(-100f,220), 320f)
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Left, new Vector2(-100f,220), 320f)
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
                            exitFactory: () => new DirectionalExit(CardinalDirection.Right, new Vector2(CompileAndConquer.ScreenSize.X+50,220), 320f)
                        ) { SpawnPattern = new LineSpawnPattern(y:150) }
                    },
            
            
                    // STEP: Two horizontal lines (both at once, parallel)
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
                            exitFactory: () => new FromTopExit(targetY: -100, speed: 100f)
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
                            exitFactory: () => new FromTopExit(targetY: -100, speed: 100f)
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
                                () => new TimedStraightAttack(dir => new StraightBullet(dir, 300f), 0.5f)
                            ),
                            spawnCount: 6,
                            duration: 6,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Left, target: finalPos, speed: 120f),
                            exitFactory: () => new FromLeftExit(targetX: -100f, speed: 120f)
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
                            exitFactory: () => new FromRightExit(targetX: 100f, speed: 120f)
                        ) { SpawnPattern = new PyramidSpawnPattern(rows: 3, spacing: 40f, y: 200f) },
                    },
*/
					// FINAL BOSS
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
                            exitFactory: () => new FromLeftExit(targetX: -100f, speed: 120f)
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

            if (waveQueue.Count == 0)
            {
                Survived?.Invoke();
                return;
            }
        
            if (currentWave == null)
                currentWave = waveQueue.Peek();
        
            // Are we done with this wave?
            if (currentWave.CurrentStep >= currentWave.Steps.Count)
            {
                waveQueue.Dequeue();
                currentWave = waveQueue.Count > 0 ? waveQueue.Peek() : null;
                return;
            }
        
            var parallelEvents = currentWave.Steps[currentWave.CurrentStep];

            //Console.WriteLine($"[WaveManager] Step: {currentWave.CurrentStep}/{currentWave.Steps.Count}");
            foreach (var (evt, idx) in parallelEvents.Select((e, i) => (e, i)))
            {
                //Console.WriteLine($"  Event {idx}: State={evt.State} Entities={evt.EventEntities.Count} Alive={evt.EventEntities.Count(e => e.isAlive)}");
            }

        
            // Spawn any not-started events in this step
            foreach (var waveEvent in parallelEvents)
            {
                if (waveEvent.State == WaveEventState.NotStarted)
                {
                    SpawnEvent(waveEvent);
                    waveEvent.State = WaveEventState.Entering;
                    waveEvent.StateTimer = 0;
                }
        
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
                        if (waveEvent.StateTimer >= waveEvent.duration || waveEvent.EventEntities.All(e => !e.isAlive))
                        {
                            foreach (var ent in waveEvent.EventEntities.Where(e => e.isAlive && !e.IsExiting))
                                ent.IsExiting = true;
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
        
                // Always update all entities
                foreach (var ent in waveEvent.EventEntities.ToList())
                    ent.Update(gameTime);
            }

            foreach (var waveEvent in parallelEvents)
                //Console.WriteLine($"Event: {waveEvent.Type.GetType().Name}, Alive: {waveEvent.EventEntities.Count(e => e.isAlive)}");

        
            // If all parallel events in this step are complete, advance to next step
            if (parallelEvents.All(ev => ev.State == WaveEventState.Complete))
                currentWave.CurrentStep++;
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
                enemy.ExitBehavior = ev.CreateExit();

                EntityHandler.Add(enemy);
                ev.EventEntities.Add(enemy);
            }
            ev.IsSpawned = true;
        }

    }
}
