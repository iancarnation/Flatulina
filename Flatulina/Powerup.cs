using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flatulina
{
    class Powerup
    {
        public Texture2D tex;
        public Vector2 position;
        public Rectangle boundingBox;
        public Color color; 
        public Powerup() { }
        public Powerup(Texture2D setTex, Vector2 setPos, int width, int height)
        {
            color = Color.White;
            tex = setTex;
            position = setPos;
            boundingBox = new Rectangle(0, 0, width, height);
        }
        public void SetPosition(Vector2 newPos)
        {
            position = newPos;
            boundingBox.X = (int)newPos.X;
            boundingBox.Y = (int)newPos.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, boundingBox, color);  
        }

        /*public void Initialize(Texture2D m_tex, Vector2 m_setPos)
        {
            color = Color.White;

            tex = m_tex;
            position = m_setPos;
        }*/
        ~Powerup() { }
    }
}
