using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using CompileAndConquer.Content.EnemyWaveManager;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Entities.Enemy_Factories;
using Microsoft.Xna.Framework;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.Content.AttackPatterns;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.SpawnPatterns;

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

        private readonly List<BossPhase> bossPhases = JSONHandler.ParseBossPhases();


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


        // Loading waves from JSON using JSONHandler
        public void LoadWaves()
        {
            waveQueue.Clear();
            var wavesArr = JSONHandler.GetWaves();
            if (wavesArr == null) return;

            foreach (var waveNode in wavesArr)
            {
                var wave = new Wave();
                var stepsArr = waveNode?["Events"]?.AsArray();
                if (stepsArr == null) continue;

                foreach (var stepArr in stepsArr)
                {
                    var stepEvents = new List<WaveEvent>();
                    foreach (var eventNode in stepArr.AsArray())
                    {
                        string enemyType = eventNode?["EnemyType"]?.ToString();
                        IEnemyFactory factory = enemyType switch
                        {
                            "TypeA" => new TypeAFactory(
                                TextureHandler.TypeAEnemy,
                                Vector2.Zero,
                                () => JSONHandler.ParseMovement(eventNode["Movement"]),
                                () => JSONHandler.ParseAttack(eventNode["Attack"])
                            ),
                            "TypeB" => new TypeBFactory(
                                TextureHandler.TypeBEnemy,
                                Vector2.Zero,
                                () => JSONHandler.ParseMovement(eventNode["Movement"]),
                                () => JSONHandler.ParseAttack(eventNode["Attack"])
                            ),
                            "MiniBoss" => new MiniBossFactory(
                                TextureHandler.MiniBoss,
                                Vector2.Zero,
                                () => JSONHandler.ParseMovement(eventNode["Movement"]),
                                () => JSONHandler.ParseAttack(eventNode["Attack"])
                            ),
                            "FinalBoss" => new FinalBossFactory(
                                TextureHandler.FinalBoss,
                                Vector2.Zero,
                                bossPhases,
                                bossPhases[0].Attack
                            ),
                            _ => null
                        };
                        if (factory == null) continue;

                        int spawnCount = eventNode?["SpawnCount"]?.GetValue<int>() ?? 1;
                        int duration = eventNode?["Duration"]?.GetValue<int>() ?? 5;

                        Func<Vector2, IEntry> entryFactory = finalPos => JSONHandler.ParseEntry(eventNode["Entry"], finalPos);
                        Func<Vector2, IExit> exitFactory = finalPos => JSONHandler.ParseExit(eventNode["Exit"], finalPos);

                        ISpawnPattern spawnPattern = JSONHandler.ParseSpawnPattern(eventNode["SpawnPattern"]);

                        var waveEvent = new WaveEvent(factory, spawnCount, duration, entryFactory, exitFactory)
                        {
                            SpawnPattern = spawnPattern
                        };
                        stepEvents.Add(waveEvent);
                    }
                    wave.Steps.Add(stepEvents);
                }
                waveQueue.Enqueue(wave);
            }
            currentWave = waveQueue.Count > 0 ? waveQueue.Peek() : null;
        }


        // Start the next wave in the queue
        public void StartWave()
        {
            currentWave = waveQueue.Peek();
        }

        // End the current wave and any subevents still running, then start the next wave
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
