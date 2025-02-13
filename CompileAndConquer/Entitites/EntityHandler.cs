using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public class EntityHandler
{
    static List<Entity> entities = new List<Entity>();
    
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
        if (entity.Collider != null)
        {
            CollisionHandler.Add(entity.Collider);
        }
    }

    public static void Update()
    {
        // Need to strategically add entities so they aren't added while looping through the list
        isUpdating = true;

        foreach (var entity in entities)
            entity.Update();

        CollisionHandler.Update();
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
            if (entity.Collider != null)
            {
                entity.Collider.Draw(spriteBatch);
            }
        }
    }
}