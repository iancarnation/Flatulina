using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Flatulina
{
    class Sprite
    {
        public Texture2D tex; //texture
        public Vector2 Position //XY Pos
        {
            get;
            set; 
        }
        public Vector2 velocity
        {
            get;
            set; 
        }
        public int width;
        public int height; 
        public float scale = 0.4f;

        private KeyboardState oldState; //for input detection

        public Sprite(int w, int h, float s) //constructor 
        {
            this.width = w;
            this.height = h;
            this.scale = s; 
        }
       
        public void Update() //update sprites
        {
            CheckInput();
            UpdatePosition(); 
        }

        public void UpdatePosition()
        {
            this.Position += this.velocity;
            this.velocity = Vector2.Zero; 
        }

        public void Draw(SpriteBatch spriteBatch) //Draw for sprites
        {
            spriteBatch.Draw(this.tex, this.Position, null, Color.White, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
        }

        public void CheckInput()
        {
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.Right) || newState.IsKeyDown(Keys.D))
            {
                this.velocity += new Vector2(4.0f, 0.0f); 
            }
            if (newState.IsKeyDown(Keys.Left) || newState.IsKeyDown(Keys.A))
            {
                this.velocity += new Vector2(-4.0f, 0.0f); 
            }
            if (newState.IsKeyDown(Keys.Up) || newState.IsKeyDown(Keys.W))
            {
                this.velocity += new Vector2(0.0f, -4.0f); 
            }
            if (newState.IsKeyDown(Keys.Down) || newState.IsKeyDown(Keys.S))
            {
                this.velocity += new Vector2(0.0f, 4.0f); 
            }
            oldState = newState; //reassign to prevent unwanted key presses
        }
    }
}
