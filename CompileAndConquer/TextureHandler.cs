using System.Dynamic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer;

public class TextureHandler
{
    public static Texture2D Player { get; set; }
    public static Texture2D Bullet { get; set; }

    public static void Load(ContentManager content)
    {
        Player = content.Load<Texture2D>("spaceship");
        Bullet = content.Load<Texture2D>("bullet");
    }
}