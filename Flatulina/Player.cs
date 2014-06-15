using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flatulina
{
    class Player
    {
        // temp
        public Color color;
        public float scale;

        // Animations

        // Sounds

        public Flatulina_Game Game
        { get { return game; } }
        Flatulina_Game game;


        // >>>>>>>>> General Properties <<<<<<<<

        // Texture representing the player // ** to be an Animation later **
        public Texture2D playerTexture;

        // Get width of player 
        public float Width { get { return this.width * scale; } } float width;

        // Get height of player
        public float Height { get { return this.height * scale; } } float height;

        // >>>>>>>>> Physics Properties <<<<<<<<<<<<<<<

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        } 
        Vector2 position;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        // >>>>>>>>> Movement Constants <<<<<<<<<<<<<<<<<

        // controlling horizontal movement
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;

        // Input configuration
        private const float MoveStickScale = 1.0f;
        private const Buttons JumpButton = Buttons.A;

        //// maximum velocity allowed
        //public Vector2 maxVelocity;  // might need for tempering jetFart

        // >>>>>>>>> Player States <<<<<<<<<<<<<<<<<

        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        /// <summary>
        /// Current user movement input.
        /// </summary>
        private float movement;

        // Jumping State

        // True if currently jumping (prevents double jump)
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        // True if jump key is currently held down
        public bool jumpKeyDown, jetKeyDown;

        // ------------ Collision Fields ------------------------

        // Area for broad collision detection
        public BoundingRect BoundingBox; // consider adding accessor that returns new BoundingRect with current position... possibly eliminates need for explicitly updating the bounding boxes, though we may still need that amongst collision steps

        // Specific collision areas for regions of player
        public BoundingRect CollisionTop, CollisionBottom, CollisionLeft, CollisionRight;

        public float thirdOfWidth, halfOfWidth, quarterOfHeight, halfOfHeight;

        // >>>>>>>>>>>>>>>>>>>> Input <<<<<<<<<<<<<<<<<<<<<<<<<<<

        


        // vvvvvvvvvvvv Yet to be organized vvvvvvvvvvvvvvvvv

        public Vector2 jetForce;

        public Rectangle fuelOutline, fuelFill; // for drawing fuel meter

        public int fuel;

        // Amount of hit points 
        public int health;

        // State of the player
        public bool Active;

        // vvvvvvvvvv TRASH vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
       
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^


        public void Initialize(Flatulina_Game game, Texture2D a_texture, Vector2 a_position )
        {
            // temp
            color = Color.White;
            scale = 0.4f;

            this.game = game;

            // set texture // ** to be animation later **
            playerTexture = a_texture;

            // Set starting position
            Position = a_position;

            // original width and height of texture
            width = playerTexture.Width;
            height = playerTexture.Height;

            // Set health
            
            
            // vvvvvvvvvvvvvvvv old properties vvvvvvvvvvvvvv
            //// Set velocity
            //Velocity = new Vector2(0.0f, 0.0f);
            //// Set accel/decel
            //accX = 0.15f * mScale;
            //decX = 0.2f * mScale;
            //// Set jump velocity
            //jumpVelocityY = 8.0f * mScale;
            //// Set jet velocity
            //jetForce = new Vector2(1.2f * mScale, -3.0f * mScale);
            //// Set max velocity
            //maxVelocity = new Vector2(3.0f * mScale, 8.0f * mScale);

            //// Set gravity
            //gravityAccel = 0.4f * mScale;
            
            //// Set states
            //jumping = false;
            //jumpKeyDown = false;

            // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            // set collision area calculation variables
            thirdOfWidth = Width / 3f;
            halfOfWidth = Width / 2f;
            quarterOfHeight = Height / 4f;
            halfOfHeight = Height / 2f;

            // set smaller collision areas
            CollisionTop = new BoundingRect(this.Position.X + thirdOfWidth, this.Position.Y - 5f, thirdOfWidth, quarterOfHeight + 10f);
            CollisionBottom = new BoundingRect(this.Position.X + thirdOfWidth, this.Position.Y + quarterOfHeight * 3f, thirdOfWidth, quarterOfHeight + 5f);
            CollisionLeft = new BoundingRect(this.Position.X, this.Position.Y + quarterOfHeight, halfOfWidth, halfOfHeight);
            CollisionRight = new BoundingRect(this.Position.X + halfOfWidth, this.Position.Y + quarterOfHeight, halfOfWidth, halfOfHeight);

            // set broad collision area
            BoundingBox = new BoundingRect(a_position.X, CollisionTop.Position.Y, this.Width, this.Height + 10f);

            // Set the player to be Active  // ** to be removed **
            Active = true;

            // Set player health
            health = 100;

            fuel = 100;

            fuelOutline = new Rectangle(8, 64, 201, 20);
            fuelFill = new Rectangle(9, 64, fuelFill.X + (fuel * 25), 19);


        }

        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        /// <remarks>
        /// We pass in all of the input states so that our game is only polling the hardware once per frame. 

        /// </remarks>
        public void Update(
            GameTime gameTime, 
            KeyboardState keyboardState,
            GamePadState gamePadState) 
        {
            GetInput(keyboardState, gamePadState);

            ApplyPhysics(gameTime);

            // check if alive and on ground -> play run/idle animation

            // Clear input
            movement = 0.0f;
            isJumping = false;

            // these here?
            // UPDATE FUEL HUD
            this.fuelFill.Width = this.fuel * 2;

            UpdateBoundingBoxes();
            
        }

        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private void GetInput(
            KeyboardState keyboardState,
            GamePadState gamePadState)
        {
            // get analog horizontal movement
            //movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            // Ignore small movements to prevent running in place
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            // Check for digital horizontal input
            if (gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
                movement = -1.0f;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadRight) ||
                    keyboardState.IsKeyDown(Keys.Right) ||
                    keyboardState.IsKeyDown(Keys.D))
            {
                movement = 1.0f;
            }

            // Check if the player wants to jump
            isJumping =
                /*gamePadState.IsButtonDown(JumpButton) ||*/
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);
           
        }

        /// <summary>
        /// Updates the player's velocity and position based on input, gravity, etc.
        /// </summary>
        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity
            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);

            // Apply pswudo-drag horizontally
            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Prevent the player from running faster than this top speed
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity
            Position += velocity * elapsed;

            // If the player is now colliding with the level, separate them
            HandleCollisions();

            // If the collision stopped us from moving, reset velocity to zero
            if (Position.X == previousPosition.X)
                velocity.X = 0f;
            if (Position.Y == previousPosition.Y)
                velocity.Y = 0f;
        }

        /// <summary>
        /// Calculates the Y velocity accounting for jumping and
        /// animates accordingly.
        /// </summary>
        /// <remarks>
        /// During the accent of a jump, the Y velocity is completely
        /// overridden by a power curve. During the decent, gravity takes
        /// over. The jump velocity is controlled by the jumpTime field
        /// which measures time into the accent of the current jump.
        /// </remarks>
        /// <param name="velocityY">
        /// The player's current velocity along the Y axis.
        /// </param>
        /// <returns>
        /// A new Y velocity if beginning or continuing a jump.
        /// Otherwise, the existing Y velocity.
        /// </returns>
        private float DoJump(float velocityY, GameTime gameTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    //if (jumpTime == 0.0f)
                        // play jump sound

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    // play jump animation
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else 
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        /// <summary>
        /// Detects and resolves all collisions between the player and his environment
        /// </summary>
        private void HandleCollisions()
        {
            // Reset flag to search for ground collision.
            isOnGround = false;

            // for the debug box
            bool hitSomething = false;

            CollisionTop.DebugRectColor = Color.Red;
            CollisionBottom.DebugRectColor = Color.Red;
            CollisionLeft.DebugRectColor = Color.Red;
            CollisionRight.DebugRectColor = Color.Red;

            // for each of the collision solids in the environment..
            for (int i = 0; i < game.collisionSolids.Count; i++)
            {
                //Console.WriteLine("Player" + player.BoundingBox.Position);
                //Console.WriteLine("Solid" + collisionSolids[i].Position);

                // check to see if player's general bounding box is colliding
                if (this.BoundingBox.Intersects(game.collisionSolids[i].BoundingBox))
                {
                    Console.WriteLine("Bounding Box Intersection");
                    this.BoundingBox.DebugRectColor = Color.Yellow;
                    hitSomething = true;

                    // run the player's collision area checks and adjust position accordingly
                    this.HandleCollisionWithSolid(game.collisionSolids[i].BoundingBox);
                }

                if (!hitSomething)
                    this.BoundingBox.DebugRectColor = Color.Red;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, Position, null, color, 0f, Vector2.Zero, scale , SpriteEffects.None, 0f);
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
                    this.isOnGround = true;
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
            this.BoundingBox.UpdatePosition(new Vector2(this.Position.X, this.Position.Y - 5f));

            this.CollisionTop.UpdatePosition(new Vector2(this.Position.X + this.thirdOfWidth, this.Position.Y - 5f));
            this.CollisionBottom.UpdatePosition(new Vector2(this.Position.X + this.thirdOfWidth, this.Position.Y + this.quarterOfHeight * 3f));
            this.CollisionLeft.UpdatePosition(new Vector2(this.Position.X, this.Position.Y + this.quarterOfHeight));
            this.CollisionRight.UpdatePosition(new Vector2(this.Position.X + this.halfOfWidth, this.Position.Y + this.quarterOfHeight));
        }

    }
}
