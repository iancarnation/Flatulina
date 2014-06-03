//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flatulina
{
    class EnvironmentSolid
    {
        public Texture2D EnvTexture;

        public Vector2 Position;

        // Area for collision detection
        public Rectangle HitBox;

        // bool for going through bottom?

        public int Width
        {
            get { return EnvTexture.Width; }
        }

        public int Height
        {
            get { return EnvTexture.Height; }
        }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            EnvTexture = texture;
            Position = position;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(EnvTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
