using System.Dynamic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer.Content;

public class TextureHandler
{
    public static Texture2D Player { get; set; }
    public static Texture2D TypeAEnemy { get; set; }
    public static Texture2D TypeBEnemy { get; set; }
	public static Texture2D Bullet { get; set; }
	public static Texture2D MiniBoss { get; set; }
	public static Texture2D FinalBoss { get; set; }
    public static Texture2D SideBarStar {get;set;}
    public static Texture2D Pixel { get; set; }

    public static void Load(ContentManager content, GraphicsDevice graphicsDevice)
    {
        Player = content.Load<Texture2D>("spaceship");
        TypeAEnemy = content.Load<Texture2D>("alien");
		TypeBEnemy = content.Load<Texture2D>("typeB");
		Bullet = content.Load<Texture2D>("bullet");
		MiniBoss = content.Load<Texture2D>("miniboss");
		FinalBoss = content.Load<Texture2D>("finalboss");
        SideBarStar = content.Load<Texture2D>("star");
        Pixel = new Texture2D(graphicsDevice, 1, 1);
        Pixel.SetData(new[] { Microsoft.Xna.Framework.Color.Transparent });
    }
}