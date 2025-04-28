using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Entities.Enemy_Subtypes;
using CompileAndConquer.Content;
using CompileAndConquer.Content.Interfaces;
using System.Collections.Generic;



namespace CompileAndConquer.Entities.Enemy_Factories
{
    public class FinalBossFactory : IEnemyFactory
    {
        public Texture2D texture;
        public Vector2 position;
        public List<BossPhase> phases;
        public IAttackPattern attack;

        public FinalBossFactory(Texture2D texture, Vector2 position, List<BossPhase> phases, IAttackPattern attack)
        {
            this.texture = texture;
            this.position = position;
            this.phases = phases;
            this.attack = attack;
        }

        public Enemy CreateEnemy()
        {
            return new FinalBoss(texture, position, phases, attack);
        }
    }
}
