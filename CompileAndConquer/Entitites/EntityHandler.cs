using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public class EntityHandler
{
    static List<Entity> entities = new List<Entity>();
    
    public static void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public static void Update()
    {
        foreach (var entity in entities)
            entity.Update();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (var entity in entities)
            entity.Draw(spriteBatch);
    }
}