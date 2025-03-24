using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;


namespace CompileAndConquer.Content;

public class EntityHandler
{
    static public List<Entity> entities = new List<Entity>();

    static List<Entity> toAddList = new List<Entity>();
    static List<Entity> toRemoveList = new List<Entity>();  
    static bool clearEnemiesScheduled = false;
    
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

    private static void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public static void Update(GameTime gameTime)
    {
        // Need to strategically add entities so they aren't added while looping through the list
        isUpdating = true;

        foreach (var entity in entities)
        {
            entity.Update(gameTime);
            entity.UpdateCollider();
        }   
        
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
