using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entities.Enemy_Factories;
using Microsoft.Xna.Framework;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.Content.AttackPatterns;
using CompileAndConquer.SpawnPatterns;
using CompileAndConquer.Content.EnemyWaveManager;




namespace CompileAndConquer.Content.EnemyWaveManager
{
    public class WaveManager
    {
        public class Wave
        {
            // Each step is a group of events that spawn together
            public List<List<WaveEvent>> Steps { get; set; }
            public int CurrentStep { get; set; }
            public string WaveID { get; set; }
        
            public Wave()
            {
                Steps = new List<List<WaveEvent>>();
                CurrentStep = 0;
                WaveID = "Wave";
            }
        }

        private Queue<Wave> waveQueue = new();
        public Action Survived { get; set; }


        private Wave currentWave;
       // private double waveTimer = 0;


		private readonly List<BossPhase> bossPhases = new List<BossPhase>
        {
            new BossPhase {
                Movement = new FinalBossMovement(),
                Attack = new AlternatingAttackPattern(
                    pauseBetweenAttacks: 15f,
                    new ThreeWaySpiralAttack(),
                    new ConcentricCircleAttack()
                ),
                HpThreshold = 2000
            },
            new BossPhase {
                Movement = new  miniBossMovement(),
                Attack = new AlternatingAttackPattern(
                    pauseBetweenAttacks: 5f,
                    new RepeatingSpiralAttack(),
                    new ArcAttackPattern(dir => new StraightBullet(dir, 200f), 0f)
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
            public string EventID { get; set; }

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
                EventID = "Event";
            }

            public IEntry CreateEntry(Vector2 finalPos) => _entryFactory(finalPos);
            public IExit  CreateExit(Vector2 finalPos)  => _exitFactory(finalPos);
        }


        /// <summary>
        /// Loading waves from JSON configuration file
        /// </summary>
        public void LoadWaves()
        {
            waveQueue.Clear();
            
            try
            {
                // Load waves from JSON
                waveQueue = JSONHandler.LoadWavesFromJson();
                
                if (waveQueue.Count == 0)
                {
                    // Fallback to a simple wave if JSON loading failed or returned empty
                    Console.WriteLine("Warning: No waves loaded from JSON. Using fallback wave.");
                    CreateFallbackWave();
                }
                else
                {
                    Console.WriteLine($"Successfully loaded {waveQueue.Count} waves from JSON.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading waves from JSON: {ex.Message}");
                Console.WriteLine("Using fallback wave.");
                CreateFallbackWave();
            }
            
            currentWave = waveQueue.Peek();
        }
        
        /// <summary>
        /// Creates a simple fallback wave in case JSON loading fails
        /// </summary>
        private void CreateFallbackWave()
        {
            waveQueue.Enqueue(new Wave
            {
                Steps = new List<List<WaveEvent>>
                {
                    // Simple Type A enemies
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
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(CardinalDirection.Top, 120f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    },
                    
                    // MiniBoss
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
                                () => new CircleAttack(24, 30f, dir => new StraightBullet(dir, 200f))
                            ),
                            spawnCount: 1,
                            duration: 10,
                            entryFactory: finalPos => new DirectionalEntry(dir: CardinalDirection.Top, target: finalPos, speed: 120f),
                            exitFactory:  finalPos => new DirectionalExit(CardinalDirection.Top, 220f)
                        ) { SpawnPattern = new LineSpawnPattern(y:200f) }
                    }
                }
            });
        }




        /// <summary>
        /// Start the next wave in the queue
        /// </summary>
        public void StartWave()
        {
            currentWave = waveQueue.Peek();
            Console.WriteLine($"Starting wave: {currentWave.WaveID}");
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
                    Console.WriteLine($"Spawning event: {waveEvent.EventID}");
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
