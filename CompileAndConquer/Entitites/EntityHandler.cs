using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CompileAndConquer.Content;

public class EntityHandler
{
    static List<Entity> entities = new List<Entity>();

    static List<Entity> toAddList = new List<Entity>();
    static List<Entity> toRemoveList = new List<Entity>();  
    static bool clearEnemiesScheduled = false;

    public static List<Entity> Entities => entities;

    static bool isUpdating = false;

    public static void Add(Entity entity)
    {
        if (isUpdating)
            toAddList.Add(entity);
        else 
            AddEntity(entity);
    }

    public static void RemoveEntity(Entity entity)
    {
        if (!toRemoveList.Contains(entity))
        {
            toRemoveList.Add(entity);
        }
    }

    public static void AddEntity(Entity entity)
    {
        entities.Add(entity);
        if (entity.Collider != null)
        {
            CollisionHandler.Add(entity.Collider);
        }
    }

    public static void Update()
    {
        isUpdating = true;

        foreach (var entity in entities)
            entity.Update();

        CollisionHandler.Update();
        isUpdating = false;

        foreach (var entity in toRemoveList)
            entities.Remove(entity);
        toRemoveList.Clear();

        if (clearEnemiesScheduled)
        {
            entities.RemoveAll(e => e is Enemy);
            clearEnemiesScheduled = false;  // Reset flag
        }

        foreach (var entity in toAddList)
            AddEntity(entity);
        toAddList.Clear();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (var entity in entities)
        {
            entity.Draw(spriteBatch);
            if (entity.Collider != null)
            {
                entity.Collider.Draw(spriteBatch);
            }
        }
    }

	public static void ClearEnemies()
    {
        entities.RemoveAll(e => e is Enemy);
    }

    public static void ScheduleClearEnemies()
    {
        clearEnemiesScheduled = true;
    }
}
