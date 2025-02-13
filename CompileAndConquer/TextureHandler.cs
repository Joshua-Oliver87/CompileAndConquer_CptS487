using System.Dynamic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public class TextureHandler
{
    public static Texture2D Player { get; set; }
    public static Texture2D Enemy { get; set; }

    public static void Load(ContentManager content)
    {
        Player = content.Load<Texture2D>("ball");
        Enemy = content.Load<Texture2D>("alien");
    }
}