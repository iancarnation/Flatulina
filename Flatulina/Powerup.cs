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
        public Powerup()  { }
        public Powerup(Texture2D setTex, Vector2 setPos, int width, int height)
        {
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
        ~Powerup() { }
    }
}
