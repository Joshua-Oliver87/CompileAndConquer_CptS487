using System.Linq;
using Microsoft.Xna.Framework;
using CompileAndConquer.Content;

namespace CompileAndConquer.Content;

public static class AISystem
{
    public static void Update(GameTime gameTime)
    {
        foreach (var entity in EntityHandler.entities.ToList()) // ToList() makes shallow copy of list so we can call EntityHandler.Add() w/o an error
        {
            if (entity is Enemy enemy && enemy.AIComponent != null)
            {
                switch (enemy.AIComponent.Behavior)
                {
                    case BehaviorType.DodgeBullets:
                        TryToDodge(enemy);
                        break;

                    case BehaviorType.SpawnAlliesWhenAlone:
                        if (EntityHandler.entities.Count(e => e is Enemy && e != enemy) < 2)
                        {
                            var newAlly = new TypeA(TextureHandler.TypeAEnemy, enemy.Position + new Vector2(50, 0), new CurvedMovement()); // can override movement here if needed
                            EntityHandler.Add(newAlly);
                        }
                        break;
                }
            }
        }
    }

    private static void TryToDodge(Enemy enemy)
    {
        var bullets = EntityHandler.entities
            .Where(e => e is PlayerBullet)
            .Cast<PlayerBullet>();

        foreach (var bullet in bullets)
        {
            float distance = Vector2.Distance(bullet.Position, enemy.Position);
            if (distance < 100) // Bullet is close
            {
                // Try dodging up or down
                enemy.Position.Y += CompileAndConquer.RandomGenerator.Next(0, 2) == 0 ? -30 : 30;
                break;
            }
        }
    }
}