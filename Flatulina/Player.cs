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

        // original dimensions of player texture
        public float width, height;

        // Amount of hit points 
        public int health;

        // >>>>>>>>> General Properties <<<<<<<<

        // Get width of player 
        public float Width { get { return this.width * scale; } }

        // Get height of player
        public float Height { get { return this.height * scale; } }


        // ------------ Movement Fields ------------------------

        public Vector2 velocity;

        // amount of X acceleration/deceleration to apply when player moves in X / is not moving in X
        public float accX, decX;

        // amount of upward force to apply when the player first presses jump
        public float jumpVelocityY; // ** maybe this becomes a Vec2? **

        // maximum velocity allowed
        public Vector2 maxVelocity;

        // >>>>>>>>> Movement Properties <<<<<<<<



        // ------------ Collision Fields ------------------------

        // Area for broad collision detection
        public BoundingRect BoundingBox;

        // Specific collision areas for regions of player
        public BoundingRect CollisionTop, CollisionBottom, CollisionLeft, CollisionRight;

        public float thirdOfWidth, halfOfWidth, quarterOfHeight, halfOfHeight;


        // ---------- World "Physics" Fields -------------------

        // the force of gravity (Y acceleration)
        public float gravityAccel;

        // ----------- Player States ---------------------------

        // True if currently jumping (prevents double jump)
        public bool jumping, jet;

        // True if jump key is currently held down
        public bool jumpKeyDown, jetKeyDown;

        // True if player's feet are on top of a surface
        public bool onGround;



        // vvvvvvvvvvvv Yet to be organized vvvvvvvvvvvvvvvvv

        public float jetVelocityY;

        public Rectangle fuelOutline, fuelFill; // for drawing fuel meter

        public int fuel;


        // State of the player
        public bool Active;

        // debug rectangle to draw
        //public Rectangle DebugRect;
        //public Color DebugRectColor;
        


        // vvvvvvvvvv TRASH vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
       
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        public void Initialize(Texture2D a_texture, Vector2 a_position )
        {
            // temp
            color = Color.White;
            scale = 0.4f;


            // set texture // ** to be animation later **
            playerTexture = a_texture;

            // Set starting position
            position = a_position;

            // original width and height of texture
            width = playerTexture.Width;
            height = playerTexture.Height;

            // Set health
            
            // desired framerate
            float mScale = 60.0f; // ** rename? **

            // Set velocity
            velocity = new Vector2(0.0f, 0.0f);
            // Set accel/decel
            accX = 0.15f * mScale;
            decX = 0.2f * mScale;
            // Set jump velocity
            jumpVelocityY = 8.0f * mScale;
            // Set jet velocity
            jetVelocityY = 3.0f * mScale;
            // Set max velocity
            maxVelocity = new Vector2(3.0f * mScale, 8.0f * mScale);

            // Set gravity
            gravityAccel = 0.4f * mScale;
            
            // Set states
            jumping = false;
            jumpKeyDown = false;

            // set collision area calculation variables
            thirdOfWidth = Width / 3f;
            halfOfWidth = Width / 2f;
            quarterOfHeight = Height / 4f;
            halfOfHeight = Height / 2f;

            // set smaller collision areas
            CollisionTop = new BoundingRect(this.position.X + thirdOfWidth, this.position.Y - 5f, thirdOfWidth, quarterOfHeight + 10f);
            CollisionBottom = new BoundingRect(this.position.X + thirdOfWidth, this.position.Y + quarterOfHeight * 3f, thirdOfWidth, quarterOfHeight + 5f);
            CollisionLeft = new BoundingRect(this.position.X, this.position.Y + quarterOfHeight, halfOfWidth, halfOfHeight);
            CollisionRight = new BoundingRect(this.position.X + halfOfWidth, this.position.Y + quarterOfHeight, halfOfWidth, halfOfHeight);

            // set broad collision area
            BoundingBox = new BoundingRect(a_position.X, CollisionTop.Position.Y, this.Width, this.Height + 10f);

            // Set the player to be Active
            Active = true;

            // Set player health
            health = 100;

            fuel = 100;

            fuelOutline = new Rectangle(8, 64, 201, 20);
            fuelFill = new Rectangle(9, 64, fuelFill.X + (fuel * 25), 19);


        }

        public void Update()
        {
 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, position, null, color, 0f, Vector2.Zero, scale , SpriteEffects.None, 0f);
        }

        public void HandleCollisionWithSolid(BoundingRect solidRect)
        {
            // need to recalc in case other collisions moved the body this frame
            UpdateBoundingBoxes();

            if (CollisionTop.Intersects(solidRect))
            {
                Console.WriteLine("Top Hit");
                CollisionTop.DebugRectColor = Color.Green;
                position.Y = solidRect.Position.Y - Height;
                if (this.velocity.Y < 0f)
                    this.velocity.Y = 0f;
            }
            else
                CollisionTop.DebugRectColor = Color.Red;

            if (CollisionBottom.Intersects(solidRect))
            {
                Console.WriteLine("Bottom Hit");
                CollisionBottom.DebugRectColor = Color.Green;
                position.Y = solidRect.Position.Y - this.Height - 1;
                if (this.velocity.Y > 0f)
                {
                    this.velocity.Y = 0f;
                    this.onGround = true;
                }
            }
            else
                CollisionBottom.DebugRectColor = Color.Red;

            if (CollisionLeft.Intersects(solidRect))
            {
                Console.WriteLine("Left Hit");
                CollisionLeft.DebugRectColor = Color.Green;
                position.X = solidRect.Position.X + solidRect.Width;
                if (this.velocity.X < 0f)
                    this.velocity.X = 0f;
                
               
            }
            else
                CollisionLeft.DebugRectColor = Color.Red;

            if (CollisionRight.Intersects(solidRect))
            {
                Console.WriteLine("Right Hit");
                CollisionRight.DebugRectColor = Color.Green;
                if (this.velocity.X > 0f)
                    this.velocity.X = 0f;
                position.X = solidRect.Position.X - Width;
            }
            else
                CollisionRight.DebugRectColor = Color.Red;

        }

        // Update Bounding Box and Collision Areas
        public void UpdateBoundingBoxes()
        {
            this.BoundingBox.UpdatePosition(new Vector2(this.position.X, this.position.Y - 5f));

            this.CollisionTop.UpdatePosition(new Vector2(this.position.X + this.thirdOfWidth, this.position.Y - 5f));
            this.CollisionBottom.UpdatePosition(new Vector2(this.position.X + this.thirdOfWidth, this.position.Y + this.quarterOfHeight * 3f));
            this.CollisionLeft.UpdatePosition(new Vector2(this.position.X, this.position.Y + this.quarterOfHeight));
            this.CollisionRight.UpdatePosition(new Vector2(this.position.X + this.halfOfWidth, this.position.Y + this.quarterOfHeight));
        }

    }
}
