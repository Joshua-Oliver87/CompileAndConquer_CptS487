using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public class EntityHandler
{
    static List<Entity> entities = new List<Entity>();

    public static List<Entity> Entities => entities;
    static List<Entity> Collisions = new List<Entity>();
    
    static bool isUpdating = false;
    static List<Entity> toAddList = new List<Entity>();

    public static void Add(Entity entity)
    {
        if (isUpdating)
            toAddList.Add(entity);
        else 
            AddEntity(entity);
    }
    private static void AddEntity(Entity entity)
    {
        entities.Add(entity);

        if (entity.Collidable)
            Collisions.Add(entity);
    }

    public static void Update()
    {
        // Need to strategically add entities so they aren't added while looping through the list
        isUpdating = true;

        foreach (var entity in entities)
            entity.Update();
        
        // Check collisions
        Stack<Entity> col = new Stack<Entity>();

        foreach (Entity collider in Collisions)
            col.Push(collider);
        
        while (col.Count > 0)
        {
            Entity checking = col.Pop();
            foreach (Entity toCheck in col)
            {
                if (checking.CollisionRectangle.Intersects(toCheck.CollisionRectangle))
                {
                    // COLLISIONS HERE WILL MOVE LATER
                    // Player and Enemy collision
                    if ((toCheck is Player && checking is Enemy) || (toCheck is Enemy && checking is Player))
                    {
                        Console.WriteLine("Player & Enemy Collision");
                    }
                    
                    // Bullet and Enemy collision
                    if ((toCheck is PlayerBullet && checking is Enemy) || (toCheck is Enemy && checking is PlayerBullet))
                    {
                        Console.WriteLine("Bullet & Enemy Collision");
                    }
                }
            }
        }
        
        isUpdating = false;
        
        
        // Add entities that are waiting to be added and clear list
        foreach (var entity in toAddList)
            AddEntity(entity);
        toAddList.Clear();

    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (var entity in entities)
        {
            entity.Draw(spriteBatch);
        }
    }

    public static void ClearEnemies()
    {
        entities.RemoveAll(e => e is Enemy);
    }
}