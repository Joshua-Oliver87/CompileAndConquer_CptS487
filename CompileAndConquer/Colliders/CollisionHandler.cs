using System;
using System.Collections.Generic;
using CompileAndConquer.Content;

namespace CompileAndConquer;

public class CollisionHandler
{
    static bool isUpdating = false;
    
    static List<Collider> colliders = new List<Collider>();
    static List<Collider> toAddList = new List<Collider>();
    
    public static void Add(Collider collider)
    {
        if (isUpdating)
            toAddList.Add(collider);
        else 
            AddCollider(collider);
    }
    
    private static void AddCollider(Collider collider)
    {
        colliders.Add(collider);
    }

    public static void Update()
    {
        Stack<Collider> toCheck = new Stack<Collider>();

        foreach (Collider collider in colliders)
            toCheck.Push(collider);

        while (toCheck.Count > 0)
        {
            Collider collider = toCheck.Pop();
            foreach (Collider col in toCheck)
            {
                if (collider.IsColliding(col)) 
                { 
                    // Collider math here
                    Console.WriteLine(collider.Entity.GetType() + " colliding with " + col.Entity.GetType());
                    if (col.Entity is Player)
                    {
                        // The player collided with something
                    }
                }
            }
        }
    }
}