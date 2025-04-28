using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using CompileAndConquer.Content;
using System;
using CompileAndConquer.Content;
using System.Collections.Generic;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.AttackPatterns;


namespace CompileAndConquer.Entities.Enemy_Subtypes
{
    public class FinalBoss : Enemy
    {
        private List<BossPhase> phases;
        private int currentPhaseIndex = 0;

        private IMovementStrategy movement;
        
        private IAttackPattern attackPattern;
        
        private bool isTransitioningPhase = false;
        private double phaseTransitionTimer = 0;
        private Vector2 phaseStartPosition = new Vector2(CompileAndConquer.ScreenSize.X / 2f, 100f);
        private double phaseTransitionDuration = 2.0; // 2 seconds pause between phases
        
        private readonly double firingDuration = 2.0;  // Fire for 3 seconds (longer than TypeA for boss feel)
        private readonly double cooldownDuration = 3.5; // Pause for 2 seconds (longer cooldown)
        
        public bool IsInvulnerable { get; set; } = false;
        
        private double stateTimer = 0;
        private bool isFiring = true;



        public FinalBoss(Texture2D texture, Vector2 position, List<BossPhase> phases, IAttackPattern attack, float speed = 4.5f, float scale = 1.5f)
            : base(texture, position, speed, scale, phases[0].Movement)
        {
            this.phases = phases;
            this.attackPattern = attack;
            this.health = 3000;
			attackPattern.Reset();
        }

        
        protected override void HandleAttack(GameTime gameTime)
        {
            if (!CanAttack() || isTransitioningPhase) return;

            attackPattern?.Update(this, gameTime);

            CanMove = !attackPattern.IsAttackInProgress && !isTransitioningPhase;
        }



        public override void Update(GameTime gameTime)
        {
            if (isTransitioningPhase)
            {
                IsInvulnerable = true;
                // Smooth move to center
                Position = Vector2.Lerp(Position, phaseStartPosition, 0.1f);

                phaseTransitionTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (phaseTransitionTimer >= phaseTransitionDuration)
                {
                    isTransitioningPhase = false;
                    IsInvulnerable = false;
                    CanMove = true;
                    isFiring = true;
                    attackPattern = phases[currentPhaseIndex].Attack;
					attackPattern.Reset();
                    movementStrategy = phases[currentPhaseIndex].Movement;
                    
                    stateTimer = 0;
                }
                return;
            }
            else
            {
                IsInvulnerable = false;
            }

            base.Update(gameTime);

            var curPhase = phases[currentPhaseIndex];
            if (curPhase.HpThreshold.HasValue && health <= curPhase.HpThreshold.Value && !isTransitioningPhase)
            {
                AdvancePhase();
            }

            if (!IsEntering && !IsExiting && isAlive)
            {
                Console.WriteLine($"FinalBoss: HandleAttack called. isFiring={isFiring}, CanMove={CanMove}");

                 HandleAttack(gameTime);
                    if (CanMove)
                        movementStrategy?.Move(this, gameTime);
            }


            if (health <= 0)
            {
                EntityHandler.RemoveEntity(this);
            }
        }

        private void AdvancePhase()
        {
            if (currentPhaseIndex < phases.Count - 1)
            {
                currentPhaseIndex++;
                isTransitioningPhase = true;
                phaseTransitionTimer = 0;
                // Set boss invulnerable, disable movement/attack here!
                CanMove = false;
                attackPattern = new noAttack();
            }
           
        }
        

        
        public override void Attack()
        {

        }
    }
}

