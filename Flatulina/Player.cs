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

        // ------------ General Fields ------------------------

        // Texture representing the player // ** to be an Animation later **
        public Texture2D playerTexture;

        // Position of the Player (relative to the upper left side of the screen)
        public Vector2 position;

        // Amount of hit points 
        public int health;

        // >>>>>>>>> General Properties <<<<<<<<

        // Get width of player 
        public int Width { get { return playerTexture.Width; } }

        // Get height of player
        public int Height { get { return playerTexture.Height; } }


        // ------------ Movement Fields ------------------------

        

        // amount of X acceleration/deceleration to apply when player moves in X / is not moving in X
        public float accX, accY, decX;

        // The amount of X and Y movement to occur on the next frame
        public Vector2 velocity;

        // amount of upward force to apply when the player first presses jump
        public float jumpVelocityY; // ** maybe this becomes a Vec2? **

        // maximum velocity allowed
        public Vector2 maxVelocity;

        // >>>>>>>>> Movement Properties <<<<<<<<



        // ------------ Collision Fields ------------------------

        // Area for collision detection
        public Rectangle BoundingBox;






        // ---------- World "Physics" Fields -------------------

        // the force of gravity (Y acceleration)
        public float gravityAccel;

        // ----------- Player States ---------------------------

        // True if currently jumping (prevents double jump)
        public bool jumping;

        // True if jump key is currently held down
        public bool jumpKeyDown;



        // vvvvvvvvvvvv Yet to be organized vvvvvvvvvvvvvvvvv


        // State of the player
        public bool Active;

        



        // corners of the hit box
        public Vector2 TopLeft { get { return new Vector2(BoundingBox.Left, BoundingBox.Top); } }
        public Vector2 TopRight { get { return new Vector2(BoundingBox.Right, BoundingBox.Top); } }
        public Vector2 BottomRight { get { return new Vector2(BoundingBox.Right, BoundingBox.Bottom); } }
        public Vector2 BottomLeft { get { return new Vector2(BoundingBox.Left, BoundingBox.Bottom); } }

        public List<Vector2> Corners { get { return new List<Vector2>() { TopLeft, TopRight, BottomRight, BottomLeft }; } }

        public Vector2 collidingCorner;


        public void Initialize(Texture2D a_texture, Vector2 a_position )
        {
            // set texture // ** to be animation later **
            playerTexture = a_texture;

            // Set starting position
            position = a_position;

            // Set health
            
            // desired framerate
            float mScale = 60.0f; // ** rename? **

            // Set velocity
            velocity = new Vector2(0.0f, 0.0f);
            // Set accel/decel
            accX = 0.2f * mScale;
            accY = 0.4f * mScale; 
            decX = 0.3f * mScale;
            // Set jump velocity
            jumpVelocityY = 64.0f * mScale;
            // Set max velocity
            maxVelocity = new Vector2(5.0f * mScale, 12.0f * mScale);

            // Set gravity
            gravityAccel = 0.5f * mScale;
            
            // Set states
            jumping = false;
            jumpKeyDown = false;




            BoundingBox = new Rectangle((int)a_position.X, (int)a_position.Y, (int)(Width * scale), (int)(Height * scale));




            // Set the player to be Active
            Active = true;

            // Set player health
            health = 100;

            collidingCorner = new Vector2(0, 0);

            // temp
            color = Color.White;
            scale = 0.5f;

        }

        public void Update()
        {
 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, position, null, color, 0f, Vector2.Zero, scale , SpriteEffects.None, 0f);
        }
    }
}
