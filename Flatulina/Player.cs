using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flatulina
{
    class Player
    {
        // temp
        public Color color;
        public float scale;

        // Animation representing the player
        public Texture2D PlayerTexture;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        //public Vector2 Velocity;

        // Area for collision detection
        public Rectangle HitBox;

        // State of the player
        public bool Active;

        // Amount of hit points 
        public int Health;

        // Get the width of the player
        public int Width
        {
            get { return PlayerTexture.Width; }
        }

        // Get height of player
        public int Height
        {
            get { return PlayerTexture.Height; }
        }

        // corners of the hit box
        public Vector2 TopLeft { get { return new Vector2(HitBox.Left, HitBox.Top); } }
        public Vector2 TopRight { get { return new Vector2(HitBox.Right, HitBox.Top); } }
        public Vector2 BottomRight { get { return new Vector2(HitBox.Right, HitBox.Bottom); } }
        public Vector2 BottomLeft { get { return new Vector2(HitBox.Left, HitBox.Bottom); } }

        public List<Vector2> Corners { get { return new List<Vector2>() { TopLeft, TopRight, BottomRight, BottomLeft }; } }

        public Vector2 collidingCorner;


        public void Initialize(Texture2D texture, Vector2 position)
        {
            // temp
            color = Color.White;
            scale = 0.5f;

            PlayerTexture = texture;

            // Set starting position to approx. middle of screen and to the back
            Position = position;

            HitBox = new Rectangle((int)Position.X, (int)Position.Y, (int)(Width * scale), (int)(Height * scale));

            // Set the player to be Active
            Active = true;

            // Set player health
            Health = 100;

            collidingCorner = new Vector2(0, 0);
        }

        public void Update()
        {
 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, color, 0f, Vector2.Zero, scale , SpriteEffects.None, 0f);
        }
    }
}
