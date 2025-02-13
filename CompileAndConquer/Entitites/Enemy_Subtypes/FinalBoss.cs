using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompileAndConquer.Content;

namespace CompileAndConquer.Entities.Enemy_Subtypes;

// Strongest enemy and can summon other enemies 
public class FinalBoss : Enemy
{
    private int phase = 1;
    private int attackTimer = 0;
    public FinalBoss(Texture2D texture, Vector2 position) : base(texture, position, 1.0f)
    {
        this.health = 1000; // default
    }

    // Move unpredictably
    public override void Update()
    {
        this.Position.X += (float)(Speed * (Game1.RandomGenerator.NextDouble() - 0.5));
        this.Position.Y += (float)(Speed * (Game1.RandomGenerator.NextDouble() - 0.5));
        
        // change phase at some condition
        if (this.health < 500 && phase == 1)
        {
            phase = 2;
            Speed = 2.0f;
        }

        attackTimer++;
        if (attackTimer % 150 == 0)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        // if phase 1 do range attack

        if (phase == 1)
        {
            //Game1.SpawnProjectile(this.Position, Player.Instance.Position);
        }
        else
        {
            // summon other enemies

            for (int i = 0; i < 3; i++)
            {
                EntityHandler.AddEntity(new TypeA(TextureHandler.Enemy,
                    new Vector2(this.Position.X + i * 20, this.Position.Y)));
            }
        }
    }
}