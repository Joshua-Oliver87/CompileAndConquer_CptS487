using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompileAndConquer;

public class Collider
{
    // Height and width of the Collider
    private int height;
    private int width;

    public bool DebugMode = false;
    
    public Entity Entity { get; set; }
    
    public Collider(int height, int width, Entity entity)
    {
        this.height = height;
        this.width = width;
        this.Entity = entity;
    }
    
    public bool IsColliding(Collider collider)
    {

        if (Math.Abs(this.Entity.Position.X - collider.Entity.Position.X) < (this.width / 2 + collider.width / 2))
        {
            if (Math.Abs(this.Entity.Position.Y - collider.Entity.Position.Y) < (this.height / 2 + collider.height / 2))
            {
                return true; 
            }
        }
        
        return false;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (DebugMode)
        {
            Texture2D _texture;
            _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.DarkGreen });

            spriteBatch.Draw(_texture,
                new Rectangle((int)Entity.Position.X - width / 2, (int)Entity.Position.Y - height / 2, width, height),
                Color.White * 0.5f);
        }
    }}